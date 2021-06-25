using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core.DTOs;

namespace NewsAggregator.DAL.CQRS.Queries.RssSourceQueries
{
    public class GetAllRssSourcesQuery : IRequest<IEnumerable<RssSourceDto>>
    {
        
    }
}
