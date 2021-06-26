using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core.DTOs;

namespace NewsAggregator.DAL.CQRS.Queries.NewsQueries
{
    public class GetNewsByIdQuery : IRequest<NewsDto>
    {
        public Guid Id { get; set; }
        public GetNewsByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
