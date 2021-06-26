using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core.DTOs;

namespace NewsAggregator.DAL.CQRS.Commands.RssSourceCommand
{
    public class AddSourceCommand : IRequest<int>
    {
        public RssSourceDto RssSourceDto { get; set; }

        public AddSourceCommand(RssSourceDto rssSourceDto)
        {
            RssSourceDto = rssSourceDto;
        }
    }
}
