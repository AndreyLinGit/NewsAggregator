using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.DTOs;

namespace NewsAggregator.DAL.Servises.Interfaces
{
    public interface IRssSourseServise
    {
        Task<List<NewsDto>> GetNewsFromSourse();
    }
}
