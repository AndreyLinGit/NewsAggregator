using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Serilog;
using Serilog.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.OLE.Interop;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Core.LemmatizationClasses;
using NewsAggregator.DAL.CQRS.Commands.NewsCommand;
using NewsAggregator.DAL.CQRS.Queries.NewsQueries;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Serviсes.Interfaces;
using Newtonsoft.Json;

namespace NewsAggregator.DAL.Serviсes.Implementation
{
    public class NewsService : INewsService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public NewsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<IEnumerable<NewsWithRssSourceNameDto>> GetPartOfNews(int count, string lastGottenPublishTime)
        {
            DateTime time = new DateTime();
            time = DateTime.Parse(lastGottenPublishTime);
            var newsCollection = await _unitOfWork.News.FindBy(
                    news => news.PublishTime.CompareTo(time) < 0,
                    rssSourceName => rssSourceName.RssSource)
                .OrderByDescending(n => n.PublishTime)
                .Take(count)
                .ToListAsync();

            return newsCollection.Select(news => _mapper.Map<NewsWithRssSourceNameDto>(news));
        }

        public async Task AddRangeOfNews(IEnumerable<NewsDto> rangeOfNewsDtos)
        {
            await _unitOfWork.News.AddRange(rangeOfNewsDtos.Select(newsDto => _mapper.Map<News>(newsDto)));
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<NewsDto> GetNewsById(Guid id)
        {
            var news = await _unitOfWork.News.GetById(id);
            return _mapper.Map<NewsDto>(news);
        }

        public async Task<IEnumerable<string>> GetCheckUrlCollection(int checkCount)
        {
            return await _unitOfWork.News.FindBy(
                    news => news.PublishTime.CompareTo(DateTime.Now) < 0)
                .OrderByDescending(news => news.PublishTime)
                .Take(checkCount)
                .AsNoTracking()
                .Select(news => news.Url)
                .ToListAsync();
        }

        public async Task RateNews()
        {
            var ratingUdateDictionary = new Dictionary<Guid, int>();
            var responseAfterLemmatization = string.Empty;
            var model = new JsonFromLemmatization();
            var newsForRateCollection = await _unitOfWork.News.FindBy(news => news.Rating == null)
                .OrderByDescending(n => n.Rating)
                .Take(30)
                .Select(news => _mapper.Map<NewsDto>(news))
                .ToListAsync();

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

                        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post,
                            "http://api.ispras.ru/texterra/v1/nlp?targetType=lemma&apikey=e6b0b7ae0df3f9a1966fd1e079af182371800b36")
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
                        model = JsonConvert
                            .DeserializeObject<IEnumerable<JsonFromLemmatization>>(responseAfterLemmatization)
                            .FirstOrDefault();
                        foreach (var lemma in model.Annotations.Lemma)
                        {
                            if (rateDictionary.TryGetValue(lemma.Value, out wordRate))
                            {
                                rate += wordRate;
                                countOFWords++;
                            }
                        }

                        var newsWithRating =
                            newsForRateCollection.FirstOrDefault(news => news.CleanedBody.Equals(model.Text));
                        if (newsWithRating != null)
                        {
                            if (!ratingUdateDictionary.Keys.Contains(newsWithRating.Id))
                            {
                                ratingUdateDictionary.Add(newsWithRating.Id,
                                    Convert.ToInt32(Math.Round(rate / countOFWords * 100)));
                            }
                        }
                    }
                }

                foreach (var pair in ratingUdateDictionary)
                {
                    var news = await _unitOfWork.News.GetById(pair.Key);
                    news.Rating = pair.Value;
                }

                await _unitOfWork.SaveChangeAsync();
            }
        }
    }
}
