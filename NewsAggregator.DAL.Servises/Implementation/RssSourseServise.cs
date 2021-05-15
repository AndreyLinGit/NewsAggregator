using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Servises.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using NewsAggregator.DAL.Core.DTOs;
using System.ServiceModel.Syndication;

namespace NewsAggregator.DAL.Servises.Implementation
{
    public class RssSourseServise : IRssSourseServise
    {
        private readonly IUnitOfWork _unitOfWork;

        public RssSourseServise(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                            Body = syndicationItem.Summary.Text,
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
