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
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Servises.Interfaces;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace NewsAggregator.DAL.Serviсes.Implementation
{
    public class RssSourceServiсe : IRssSourceService
    {
        private readonly INewsService _newsService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebParser _onlinerParser;
        private readonly IWebParser _shazooParser;
        private readonly IWebParser _4pdaParser;

        public RssSourceServiсe(IUnitOfWork unitOfWork, WebParserResolver servserviceAccessor, INewsService newsService)
        {
            _unitOfWork = unitOfWork;
            _newsService = newsService;
            _onlinerParser = servserviceAccessor("Onliner");
            _shazooParser = servserviceAccessor("Shazoo");
            _4pdaParser = servserviceAccessor("4pda");
        }

        //public async Task<List<NewsDto>> GetNewsFromSource(bool costil)
        //{
        //    return null;
        //}

        #region MyRegion
        public async Task<List<NewsDto>> GetNewsFromSource(bool costil)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var sources = new List<string>();
            sources.Add("https://www.onliner.by/feed");
            sources.Add("https://shazoo.ru/feed/rss");
            sources.Add("https://4pda.to/feed/");

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = new ConcurrentBag<NewsDto>();

            Task[] tasks1 = new Task[3]
            {
                new Task(() =>
                {
                    using (var reader = XmlReader.Create("https://www.onliner.by/feed"))
                    {
                        SyndicationFeed feed = SyndicationFeed.Load(reader);
                        reader.Close();
                        Parallel.ForEach(feed.Items, async (syndicationItem) =>
                        {
                            var news = new NewsDto()
                            {
                                Article = syndicationItem.Title.Text,
                                Summary = await _onlinerParser.CleanSummary(syndicationItem),
                                Body = await _onlinerParser.Parse(syndicationItem.Id),
                                CleanedBody = await _onlinerParser.CleanParse(syndicationItem.Id),
                                Id = Guid.NewGuid(),
                                PublishTime = syndicationItem.PublishDate.DateTime,
                                Rating = 0,
                                Url = syndicationItem.Id
                            };
                            if (result.Count(addedNews => addedNews.Article.Contains(news.Article)) != 0)
                            {
                                result.Add(news);
                            }
                        });
                    }
                }),
                new Task(() =>
                {
                    using (var reader = XmlReader.Create("https://shazoo.ru/feed/rss"))
                    {
                        SyndicationFeed feed = SyndicationFeed.Load(reader);
                        reader.Close();
                        Parallel.ForEach(feed.Items, async (syndicationItem) =>
                        {
                            var news = new NewsDto()
                            {
                                Article = syndicationItem.Title.Text,
                                Summary = await _shazooParser.CleanSummary(syndicationItem),
                                Body = await _shazooParser.Parse(syndicationItem.Id),
                                CleanedBody = await _shazooParser.CleanParse(syndicationItem.Id),
                                Id = Guid.NewGuid(),
                                PublishTime = syndicationItem.PublishDate.DateTime,
                                Rating = 0,
                                Url = syndicationItem.Id
                            };
                            if (result.Count(addedNews => addedNews.Article.Contains(news.Article)) != 0)
                            {
                                result.Add(news);
                            }
                        });
                    }
                }),
                new Task(() =>
                {
                    using (var reader = XmlReader.Create("https://4pda.to/feed/"))
                    {
                        SyndicationFeed feed = SyndicationFeed.Load(reader);
                        reader.Close();
                        Parallel.ForEach(feed.Items, async (syndicationItem) =>
                        {
                            var news = new NewsDto()
                            {
                                Article = syndicationItem.Title.Text,
                                Summary = await _4pdaParser.CleanSummary(syndicationItem),
                                Body = await _4pdaParser.Parse(syndicationItem.Id),
                                CleanedBody = await _4pdaParser.CleanParse(syndicationItem.Id),
                                Id = Guid.NewGuid(),
                                PublishTime = syndicationItem.PublishDate.DateTime,
                                Rating = 0,
                                Url = syndicationItem.Id
                            };
                            if (result.Count(addedNews => addedNews.Article.Contains(news.Article)) == 0)
                            {
                                result.Add(news);
                            }
                            
                        });
                    }
                })
            };
            foreach (var t in tasks1)
                t.Start();
            Task.WaitAll(tasks1);

            stopwatch.Stop();
            var time = stopwatch.Elapsed.Seconds;
            //
            return result.ToList();
        }
        #endregion


        public async Task GetNewsFromSources()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var sources = _unitOfWork.RssSourse.Get().ToList();
            if (sources.Any())
            {
                var newsCollection = new ConcurrentBag<NewsDto>();
                Parallel.ForEach(sources, (source) =>
                {
                    try
                    {
                        using (var reader = XmlReader.Create(source.Url))
                        {
                            SyndicationFeed feed = SyndicationFeed.Load(reader);
                            reader.Close();
                            Parallel.ForEach(feed.Items, async (syndicationItem) =>
                            {
                                var news = new NewsDto()
                                {
                                    Article = syndicationItem.Title.Text,
                                    Summary = await SummaryParserSwitcher(source.Name,syndicationItem),
                                    Body = await ParserSwitcher(source.Name, syndicationItem.Id),
                                    CleanedBody = await CleanParserSwitcher(source.Name, syndicationItem.Id),
                                    Id = Guid.NewGuid(),
                                    PublishTime = syndicationItem.PublishDate.DateTime,
                                    Rating = 0,
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
                        //logs
                    }
                    

                });
                await _newsService.AddRangeOfNews(newsCollection);
                var time = stopwatch.Elapsed.Seconds;
            }
            else
            {
                //Write into log "hasn't available sources"
            }
        }
        public async Task AddSource(RssSourceDto source)
        {
            await _unitOfWork.RssSourse.Add(new RssSource
            {
                Id = source.Id,
                Name = source.Name,
                Url = source.Url
            });
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<IEnumerable<RssSource>> GetAllSources()
        {
            return await _unitOfWork.RssSourse.Get().ToListAsync();
        }


        public async Task<RssSource> GetSourceById(Guid id)
        {
            return await _unitOfWork.RssSourse.GetById(id);
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
