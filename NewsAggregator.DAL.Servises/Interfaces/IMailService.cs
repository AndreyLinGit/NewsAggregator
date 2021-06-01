using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DAL.Serviсes.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
