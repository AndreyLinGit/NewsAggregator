using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Serviсes.Interfaces;
using Serilog;

namespace NewsAggregator.DAL.Serviсes.Implementation
{
    public class RssSourceServiсe : IRssSourceService
    {
        private readonly IMapper _mapper;
        private readonly INewsService _newsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebParser _onlinerParser;
        private readonly IWebParser _shazooParser;
        private readonly IWebParser _4pdaParser;

        public RssSourceServiсe(IUnitOfWork unitOfWork, WebParserResolver serviceAccessor, INewsService newsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _newsService = newsService;
            _mapper = mapper;
            _onlinerParser = serviceAccessor("Onliner");
            _shazooParser = serviceAccessor("Shazoo");
            _4pdaParser = serviceAccessor("4pda");
        }

        public async Task GetNewsFromSources()
        {
            var sources = _unitOfWork.RssSourse.Get().ToList();
            if (sources.Any())
            {
                var urlCollection = await _newsService.GetCheckUrlCollection(500);
                var newsCollection = new ConcurrentBag<NewsDto>();
                Parallel.ForEach(sources, (source) =>
                {
                    try
                    {
                        using (var reader = XmlReader.Create(source.Url))
                        {
                            SyndicationFeed feed = SyndicationFeed.Load(reader);
                            reader.Close();
                            Parallel.ForEach(feed.Items.Where(item => !urlCollection.Any(url => url.Equals(item.Id))), async (syndicationItem) =>
                            {
                                var news = new NewsDto()
                                {
                                    Article = syndicationItem.Title.Text,
                                    Summary = await SummaryParserSwitcher(source.Name, syndicationItem),
                                    Body = await ParserSwitcher(source.Name, syndicationItem.Id),
                                    CleanedBody = await CleanParserSwitcher(source.Name, syndicationItem.Id),
                                    Id = Guid.NewGuid(),
                                    PublishTime = syndicationItem.PublishDate.DateTime,
                                    RssSourceId = source.Id,
                                    Url = syndicationItem.Id
                                };

                                if (news.Body != string.Empty) 
                                {
                                    newsCollection.Add(news);
                                }
                            });
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error("Aggregate was failed",e.StackTrace);
                    }
                    
                });
                await _newsService.AddRangeOfNews(newsCollection);
            }
            else
            {
                Log.Error("hasn't available sources at the database");
            }
        }


        public async Task AddSource(RssSourceDto source)
        {
            await _unitOfWork.RssSourse.Add(_mapper.Map<RssSource>(source));
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<IEnumerable<RssSourceDto>> GetAllSources()
        {
            var rssSources = await _unitOfWork.RssSourse.Get().ToListAsync();
            return rssSources.Select(source => _mapper.Map<RssSourceDto>(source));
        }

        private async Task<string> ParserSwitcher(string nameOfSource, string newsUrl)
        {
            switch (nameOfSource)
            {
                case "Onliner":
                    return await _onlinerParser.Parse(newsUrl);
                case "Shazoo":
                    return await _shazooParser.Parse(newsUrl);
                case "4pda":
                    return await _4pdaParser.Parse(newsUrl);
                default:
                    return string.Empty;
            }
        }

        private async Task<string> CleanParserSwitcher(string nameOfSource, string newsUrl)
        {
            switch (nameOfSource)
            {
                case "Onliner":
                    return await _onlinerParser.CleanParse(newsUrl);
                case "Shazoo":
                    return await _shazooParser.CleanParse(newsUrl);
                case "4pda":
                    return await _4pdaParser.CleanParse(newsUrl);
                default:
                    return string.Empty;
            }
        }

        private async Task<string> SummaryParserSwitcher(string nameOfSource, SyndicationItem item)
        {
            switch (nameOfSource)
            {
                case "Onliner":
                    return await _onlinerParser.CleanSummary(item);
                case "Shazoo":
                    return await _shazooParser.CleanSummary(item);
                case "4pda":
                    return await _4pdaParser.CleanSummary(item);
                default:
                    return string.Empty;
            }
        }

    }
}
