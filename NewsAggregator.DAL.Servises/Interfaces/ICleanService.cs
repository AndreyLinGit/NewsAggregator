using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DAL.Serviсes.Interfaces
{
    public interface ICleanService
    {
        Task<string> CleanSummary(string text);
        Task<string> CleanBody(string text);
    }
}
