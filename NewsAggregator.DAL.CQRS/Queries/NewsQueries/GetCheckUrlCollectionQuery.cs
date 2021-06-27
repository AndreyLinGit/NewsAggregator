using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace NewsAggregator.DAL.CQRS.Queries.NewsQueries
{
    public class GetCheckUrlCollectionQuery : IRequest<IEnumerable<string>>
    {
        public int CheckCount { get; set; }

        public GetCheckUrlCollectionQuery(int checkCount)
        {
            CheckCount = checkCount;
        }
    }
}
