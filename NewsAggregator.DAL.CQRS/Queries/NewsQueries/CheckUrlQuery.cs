using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace NewsAggregator.DAL.CQRS.Queries.NewsQueries
{
    public class CheckUrlQuery : IRequest<bool>
    {
        public string NewsUrl { get; set; }

        public CheckUrlQuery(string newsUrl)
        {
            NewsUrl = newsUrl;
        }
    }
}
