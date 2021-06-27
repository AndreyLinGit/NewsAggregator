using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace NewsAggregator.DAL.CQRS.Commands.NewsCommand
{
    public class SaveNewsRatingCommand : IRequest<int>
    {
        public Dictionary<Guid, int> RatingDictionary { get; set; }

        public SaveNewsRatingCommand(Dictionary<Guid, int> ratingDictionary)
        {
            RatingDictionary = ratingDictionary;
        }
    }
}
