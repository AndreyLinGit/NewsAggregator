using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core.DTOs;

namespace NewsAggregator.DAL.CQRS.Queries.NewsQueries
{
    public class GetPartOfNewsQuery : IRequest<IEnumerable<NewsWithRssSourceNameDto>>
    {
        public int Count { get; set; }
        public string DateTime { get; set; }

        public GetPartOfNewsQuery(int count, string dateTime)
        {
            Count = count;
            DateTime = dateTime;
        }
    }
}
