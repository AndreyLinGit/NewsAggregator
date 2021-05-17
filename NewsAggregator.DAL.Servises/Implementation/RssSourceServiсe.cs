using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using NewsAggregator.DAL.Core.DTOs;
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

        public async Task<List<NewsDto>> GetNewsFromSourse()
        {
            var sourses = new List<string>();
            sourses.Add("https://news.tut.by/rss/all.rss");

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
    }
}
