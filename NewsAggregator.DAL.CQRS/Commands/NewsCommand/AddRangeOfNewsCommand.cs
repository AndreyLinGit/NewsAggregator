using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core.DTOs;

namespace NewsAggregator.DAL.CQRS.Commands.NewsCommand
{
    public class AddRangeOfNewsCommand : IRequest<int>
    {
        public IEnumerable<NewsDto> NewsDtos { get; set; }

        public AddRangeOfNewsCommand(IEnumerable<NewsDto> news)
        {
            NewsDtos = news;
        }

    }
}
