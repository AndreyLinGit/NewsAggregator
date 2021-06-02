using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.DTOs;

namespace NewsAggregator.DAL.Serviсes.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);

    }
}
