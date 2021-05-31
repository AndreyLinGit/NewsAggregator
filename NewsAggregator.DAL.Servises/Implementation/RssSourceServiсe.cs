using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
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
        private readonly IWebParser _wylsaParser;
        private readonly IWebParser _igromanijaParser;

        public RssSourceServiсe(IUnitOfWork unitOfWork, WebParserResolver servserviceAccessor, INewsService newsService)
        {
            _unitOfWork = unitOfWork;
            _newsService = newsService;
            _onlinerParser = servserviceAccessor("Onliner");
            _shazooParser = servserviceAccessor("Shazoo");
            _4pdaParser = servserviceAccessor("4pda");
            _wylsaParser = servserviceAccessor("Wylsa");
            _igromanijaParser = servserviceAccessor("Igromanija");
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
            sources.Add("https://wylsa.com/feed/");
            sources.Add("https://www.igromania.ru/rss/all.rss");

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = new List<NewsDto>();

            Task[] tasks1 = new Task[5]
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
                                Body = await _onlinerParser.Parse(syndicationItem.Id),
                                Id = Guid.NewGuid(),
                                PublishTime = DateTime.Now,
                                Rating = 0,
                                Url = syndicationItem.Id
                            };
                            result.Add(news);
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
                                Body = await _shazooParser.Parse(syndicationItem.Id),
                                Id = Guid.NewGuid(),
                                PublishTime = DateTime.Now,
                                Rating = 0,
                                Url = syndicationItem.Id
                            };
                            result.Add(news);
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
                                Body = await _4pdaParser.Parse(syndicationItem.Id),
                                Id = Guid.NewGuid(),
                                PublishTime = DateTime.Now,
                                Rating = 0,
                                Url = syndicationItem.Id
                            };
                            result.Add(news);
                        });
                    }
                }),
                new Task(() =>
                {
                    using (var reader = XmlReader.Create("https://wylsa.com/feed/"))
                    {
                        SyndicationFeed feed = SyndicationFeed.Load(reader);
                        reader.Close();
                        Parallel.ForEach(feed.Items, async (syndicationItem) =>
                        {
                            var news = new NewsDto()
                            {
                                Article = syndicationItem.Title.Text,
                                Body = await _wylsaParser.Parse(syndicationItem.Id),
                                Id = Guid.NewGuid(),
                                PublishTime = DateTime.Now,
                                Rating = 0,
                                Url = syndicationItem.Id
                            };
                            result.Add(news);
                        });
                    }
                }),
                new Task(() =>
                {
                    using (var reader = XmlReader.Create("https://www.igromania.ru/rss/all.rss"))
                    {
                        SyndicationFeed feed = SyndicationFeed.Load(reader);
                        reader.Close();
                        Parallel.ForEach(feed.Items, async (syndicationItem) =>
                        {
                            var news = new NewsDto()
                            {
                                Article = syndicationItem.Title.Text,
                                Body = await _igromanijaParser.Parse(syndicationItem.Id),
                                Id = Guid.NewGuid(),
                                PublishTime = DateTime.Now,
                                Rating = 0,
                                Url = syndicationItem.Id
                            };
                            result.Add(news);
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
            return result;
        }
        #endregion


        public async Task GetNewsFromSources()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var sources = _unitOfWork.RssSourse.Get().ToList();
            if (sources.Any())
            {
                var result = new ConcurrentBag<NewsDto>();
                Parallel.ForEach(sources, (source) =>
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
                                Body = await ParserSwitcher(source.Name, syndicationItem.Id),
                                Id = Guid.NewGuid(),
                                PublishTime = DateTime.Now,
                                Rating = 0,
                                RssSourceId = source.Id,
                                Url = syndicationItem.Id
                            };
                            result.Add(news);
                        });
                    }

                });
                await _newsService.AddRangeOfNews(result);
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
                case "Wylsa":
                    return await _wylsaParser.Parse(newsUrl);
                case "Igromanija":
                    return await _igromanijaParser.Parse(newsUrl);
                default:
                    return string.Empty;
            }
        }

    }
}
