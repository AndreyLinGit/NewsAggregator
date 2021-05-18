using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
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
        private readonly IWebParser _tutbyParser;

        public RssSourceServiсe(IUnitOfWork unitOfWork, IWebParser tutbyParser)
        {
            _unitOfWork = unitOfWork;
            _tutbyParser = tutbyParser;
        }

        public async Task<List<NewsDto>> GetNewsFromSource(bool costil)
        {
            var sourses = new List<string>();
            sourses.Add("https://www.onliner.by/feed");

            var result = new List<NewsDto>();
            foreach (var sourse in sourses)
            {
                using (var reader = XmlReader.Create(sourse))
                {
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    reader.Close();
                    foreach (var syndicationItem in feed.Items)
                    {
                        var news = new NewsDto()
                        {
                            Article = syndicationItem.Title.Text,
                            Body = await _tutbyParser.Parse(syndicationItem.Id),
                            //UrlSrc = Regex.Match(syndicationItem.Summary.Text, @"<img\s+src='(.+)'\s+border='0'\s+/>").Groups[1].Value,
                            Id = Guid.NewGuid(),
                            PublishTime = DateTime.Now,
                            Rating = 0,
                            Url = syndicationItem.Id
                        };
                        result.Add(news);
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
                            Body = await _tutbyParser.Parse(syndicationItem.Id),
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
            //var result = _unitOfWork.SaveChangeAsync();
            _unitOfWork.SaveChange();
        }

        public async Task<IEnumerable<RssSource>> GetAllSources()
        {
            return await _unitOfWork.RssSourse.Get().ToListAsync();
        }
    }
}
