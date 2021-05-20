using System;
using System.Collections.Generic;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebParser _onlinerParser;
        private readonly IWebParser _shazooParser;
        private readonly IWebParser _4pdaParser;
        private readonly IWebParser _wylsaParser;
        private readonly IWebParser _igromanijaParser;

        public RssSourceServiсe(IUnitOfWork unitOfWork, WebParserResolver servserviceAccessor)
        {
            _unitOfWork = unitOfWork;
            _onlinerParser = servserviceAccessor("Onliner");
            _shazooParser = servserviceAccessor("Shazoo");
            _4pdaParser = servserviceAccessor("4pda");
            _wylsaParser = servserviceAccessor("Wylsa");
            _igromanijaParser = servserviceAccessor("Igromanija");
        }

        public async Task<List<NewsDto>> GetNewsFromSource(bool costil)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var sources = new List<string>();
            sources.Add("https://www.onliner.by/feed");
            sources.Add("https://shazoo.ru/feed/rss");
            sources.Add("https://4pda.to/feed/");
            sources.Add("https://wylsa.com/feed/");
            sources.Add("https://www.igromania.ru/rss/all.rss");

            var result = new List<NewsDto>();
            foreach (var source in sources)
            {
                using (var reader = XmlReader.Create(source))
                {
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    reader.Close();
                    if (source.Equals(sources[0])) //Id from RssSource
                    {
                        foreach (var syndicationItem in feed.Items)
                        {
                            var news = new NewsDto()
                            {
                                Article = syndicationItem.Title.Text,
                                Body = _onlinerParser.Parse(syndicationItem.Id),
                                Id = Guid.NewGuid(),
                                PublishTime = DateTime.Now,
                                Rating = 0,
                                Url = syndicationItem.Id
                            };
                            result.Add(news);
                        }
                    }
                    if (source.Equals(sources[1])) //Id from RssSource
                    {
                        foreach (var syndicationItem in feed.Items)
                        {
                            var news = new NewsDto()
                            {
                                Article = syndicationItem.Title.Text,
                                Body = _shazooParser.Parse(syndicationItem.Id),
                                Id = Guid.NewGuid(),
                                PublishTime = DateTime.Now,
                                Rating = 0,
                                Url = syndicationItem.Id
                            };
                            result.Add(news);
                        }
                    }
                    if (source.Equals(sources[2])) //Id from RssSource
                    {
                        foreach (var syndicationItem in feed.Items)
                        {
                            var news = new NewsDto()
                            {
                                Article = syndicationItem.Title.Text,
                                Body = _4pdaParser.Parse(syndicationItem.Id),
                                Id = Guid.NewGuid(),
                                PublishTime = DateTime.Now,
                                Rating = 0,
                                Url = syndicationItem.Id
                            };
                            result.Add(news);
                        }
                    }
                    if (source.Equals(sources[3])) //Id from RssSource
                    {
                        foreach (var syndicationItem in feed.Items)
                        {
                            var news = new NewsDto()
                            {
                                Article = syndicationItem.Title.Text,
                                Body = _wylsaParser.Parse(syndicationItem.Id),
                                Id = Guid.NewGuid(),
                                PublishTime = DateTime.Now,
                                Rating = 0,
                                Url = syndicationItem.Id
                            };
                            result.Add(news);
                        }
                    }
                    if (source.Equals(sources[4])) //Id from RssSource
                    {
                        foreach (var syndicationItem in feed.Items)
                        {
                            var news = new NewsDto()
                            {
                                Article = syndicationItem.Title.Text,
                                Body = _igromanijaParser.Parse(syndicationItem.Id),
                                Id = Guid.NewGuid(),
                                PublishTime = DateTime.Now,
                                Rating = 0,
                                Url = syndicationItem.Id
                            };
                            result.Add(news);
                        }
                    }

                }
            }
            //
            return result;
        }

        public async Task<List<NewsDto>> GetNewsFromSource()
        {
            var result = new List<NewsDto>();
            var sources = _unitOfWork.RssSourse.Get().ToList();
            foreach (var source in sources)
            {
                using (var reader = XmlReader.Create(source.Url))
                {
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    reader.Close();
                    foreach (var syndicationItem in feed.Items)
                    {
                        var news = new NewsDto()
                        {
                            Article = syndicationItem.Title.Text,
                            Body = _onlinerParser.Parse(syndicationItem.Id),
                            Id = Guid.NewGuid(),
                            PublishTime = DateTime.Now,
                            Rating = 0,
                            RssSourceId = source.Id,
                            Url = syndicationItem.Id
                        };
                        result.Add(news);
                    }
                }
            }
            //_unitOfWork.News.AddRange(result);
            return result;
        }

        public async Task AddSource(RssSourceDto source)
        {
            await _unitOfWork.RssSourse.Add(new RssSource
            {
                Id = source.Id,
                Name = source.Name,
                Url = source.Url
            });
            var result = await _unitOfWork.SaveChangeAsync();
            //_unitOfWork.SaveChange();
        }

        public async Task<IEnumerable<RssSource>> GetAllSources()
        {
            return await _unitOfWork.RssSourse.Get().ToListAsync();
        }
    }
}
