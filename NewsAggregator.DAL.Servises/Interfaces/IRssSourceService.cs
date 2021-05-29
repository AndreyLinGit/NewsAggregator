using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.Serviсes.Interfaces
{
    public interface IRssSourceService
    {
        Task AddSource(RssSourceDto source);
        Task<IEnumerable<RssSource>> GetAllSources();
        Task GetNewsFromSources();
        Task<List<NewsDto>> GetNewsFromSource(bool costil);
    }
}
