using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.WebEncoders.Testing;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Core.LemmatizationClasses;
using NewsAggregator.DAL.CQRS.Commands.NewsCommand;
using NewsAggregator.DAL.CQRS.Queries.NewsQueries;
using NewsAggregator.DAL.CQRS.Queries.UserQueries;
using NewsAggregator.DAL.CQRS.QueryHandlers.NewsHandlers;
using NewsAggregator.DAL.Serviсes.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Math.EC.Rfc7748;
using Serilog;

namespace NewsAggregator.DAL.Serviсes.Implementation.Cqrs
{
    public class NewsCqrsServices : INewsService
    {
        private readonly IMediator _mediator;

        public NewsCqrsServices(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<NewsWithRssSourceNameDto>> GetPartOfNews(int count, string lastGottenPublishTime)
        {
            return await _mediator.Send(new GetPartOfNewsQuery(count, lastGottenPublishTime));
        }

        public async Task AddRangeOfNews(IEnumerable<NewsDto> rangeOfNews)
        {
            await _mediator.Send(new AddRangeOfNewsCommand(rangeOfNews));
        }

        public async Task<NewsDto> GetNewsById(Guid id)
        {
            return await _mediator.Send(new GetNewsByIdQuery(id));
        }

        public async Task<IEnumerable<string>> GetCheckUrlCollection(int checkCount)
        {
            return await _mediator.Send(new GetCheckUrlCollectionQuery(checkCount));
        }

        public async Task RateNews()
        {
            var ratingUdateDictionary = new Dictionary<Guid, int>();
            var responseAfterLemmatization = string.Empty;
            var model = new JsonFromLemmatization();
            var newsForRateCollection = await _mediator.Send(new GetPartOfNewsForRateQuery(30));

            if (newsForRateCollection.Any())
            {
                double rate = 0;
                int wordRate = 0;
                int countOFWords = 1;

                var path = Environment.CurrentDirectory + @"\AFINN-ru — Encoding UTF-8.txt";
                var jsonString = File.ReadAllText(path, Encoding.UTF8);
                var rateDictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonString);

                foreach (var news in newsForRateCollection)
                {
                    using (var httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders
                            .Accept
                            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://api.ispras.ru/texterra/v1/nlp?targetType=lemma&apikey=e6b0b7ae0df3f9a1966fd1e079af182371800b36")
                        {
                            Content = new StringContent("[{\"text\":\"" + news.CleanedBody + "\"}]", Encoding.UTF8,
                                "application/json")
                        };
                        var response = httpClient.Send(request);
                        if (response.StatusCode != HttpStatusCode.InternalServerError)
                        {
                            responseAfterLemmatization = await response.Content.ReadAsStringAsync();
                        }
                        else
                        {
                            Log.Warning(string.Format("Lemmatization of news (id {0}) has failed. Can't rate it correctly. StatusCode -- {1}", news.Id, response.StatusCode));
                            ratingUdateDictionary.Add(news.Id, 0);
                        }
                    }

                    if (!string.IsNullOrEmpty(responseAfterLemmatization))
                    {
                        model = JsonConvert.DeserializeObject<IEnumerable<JsonFromLemmatization>>(responseAfterLemmatization).FirstOrDefault();
                        foreach (var lemma in model.Annotations.Lemma)
                        {
                            if (rateDictionary.TryGetValue(lemma.Value, out wordRate))
                            {
                                rate += wordRate;
                                countOFWords++;
                            }
                        }

                        var newsWithRating = newsForRateCollection.FirstOrDefault(news => news.CleanedBody.Equals(model.Text));
                        if (newsWithRating != null)
                        {
                            if (!ratingUdateDictionary.Keys.Contains(newsWithRating.Id))
                            {
                                ratingUdateDictionary.Add(newsWithRating.Id, Convert.ToInt32(Math.Round(rate / countOFWords * 100)));
                            }
                        }
                    }
                }
                var count = await _mediator.Send(new SaveNewsRatingCommand(ratingUdateDictionary));
            }
        }
    }
}
