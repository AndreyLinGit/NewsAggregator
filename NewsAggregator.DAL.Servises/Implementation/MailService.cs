using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace NewsAggregator.DAL.Serviсes.Implementation
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var filePath = Directory.GetCurrentDirectory() + "\\Views\\Account\\ConfirmLetter.html";
            var str = new StreamReader(filePath);
            var mailText = await str.ReadToEndAsync();
            str.Close();
            mailText = mailText.Replace("[Email]", mailRequest.ToEmail).Replace("[ConfirmLink]", mailRequest.Link);
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject; //?
            var builder = new BodyBuilder();
            builder.HtmlBody = mailText;
            email.Body = builder.ToMessageBody();
            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
        }
    }
}
