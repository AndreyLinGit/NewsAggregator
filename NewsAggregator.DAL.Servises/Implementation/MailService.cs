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
            string filePath = Directory.GetCurrentDirectory() + "\\Views\\Account\\htmlpage.html";
            StreamReader str = new StreamReader(filePath);
            string mailText = str.ReadToEnd();
            str.Close();
            mailText = mailText.Replace("[Email]", mailRequest.ToEmail).Replace("[ConfirmLink]", mailRequest.Link);
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
            email.To.Add(new MailboxAddress(mailRequest.ToEmail));
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


            //string FilePath = Directory.GetCurrentDirectory() + "\\Views\\Account\\htmlpage.html";
            //StreamReader str = new StreamReader(FilePath);
            //string MailText = str.ReadToEnd();
            //str.Close();
            //MailText = MailText.Replace("[Email]", request.UserName).Replace("[ConfirmLink]", request.ToEmail);

            //var emailMessage = new MimeMessage();

            //emailMessage.From.Add(new MailboxAddress("Администрация сайта NewsAggregator", "newsaggregator21@gmail.com"));
            //emailMessage.To.Add(new MailboxAddress("", email));
            //emailMessage.Subject = subject;

            //var bodyBuilder = new BodyBuilder();
            //bodyBuilder.HtmlBody = File.ReadAllText(@"D:\MyWorkProject\NewsAggregator\NewsAggregator\Views\Account\htmlpage.html");
            //emailMessage.Body = bodyBuilder.ToMessageBody();
            
            //using (var client = new SmtpClient())
            //{
            //    await client.ConnectAsync("smtp.gmail.com", 465, true);
            //    await client.AuthenticateAsync("newsaggregator21@gmail.com", "Mabel01020103");
            //    await client.SendAsync(emailMessage);

            //    await client.DisconnectAsync(true);
            //}

        }
    }
}
