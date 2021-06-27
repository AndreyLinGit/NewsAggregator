using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.CQRS.Queries.NewsQueries
{
    public class GetPartOfNewsForRateQuery : IRequest<IEnumerable<NewsDto>>
    {
        public int Count { get; set; }

        public GetPartOfNewsForRateQuery(int count)
        {
            Count = count;
        }
    }
}
