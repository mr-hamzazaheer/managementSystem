using Microsoft.Extensions.Options;
using Shared.Common;
using Shared.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    public class SendEmail : IEmailSenderService
    {
        private  SMTP _settings { get; }

        public SendEmail(IOptions<SMTP> settings)
        {
            _settings = settings.Value;
        }

        public async Task SendEmailAsync(string from,string to,string subject,string bodyHtml,List<string>? cc = null,List<string>? bcc = null, List<(byte[] Content, string FileName, string ContentType)>? attachments = null)
        {
            try
            {
                var fromAddress = new MailAddress("hamza.zaheer.work@gmail.com", "Hamza Zaheer");
                var message = new MailMessage
                {
                    From = fromAddress,
                    Subject = subject,
                    IsBodyHtml = true
                };

                message.To.Add(to);
                cc?.ForEach(addr => message.CC.Add(addr));
                bcc?.ForEach(addr => message.Bcc.Add(addr));

                var bodyView = AlternateView.CreateAlternateViewFromString(bodyHtml, null, "text/html");

                //if (inlineImages != null)
                //{
                //    foreach (var (contentId, imageBytes) in inlineImages)
                //    {
                //        var stream = new MemoryStream(imageBytes);
                //        var linkedResource = new LinkedResource(stream, "image/png")
                //        {
                //            ContentId = contentId,
                //            TransferEncoding = System.Net.Mime.TransferEncoding.Base64
                //        };
                //        bodyView.LinkedResources.Add(linkedResource);
                //    }
                //}

                message.AlternateViews.Add(bodyView);

                if (attachments != null)
                {
                    foreach (var (content, fileName, contentType) in attachments)
                    {
                        var stream = new MemoryStream(content);
                        var attachment = new Attachment(stream, fileName, contentType);
                        message.Attachments.Add(attachment);
                    }
                }

                using var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    Credentials = new NetworkCredential("hamza.zaheer.work@gmail.com", "umqn xieb kjhu skiq")
                };

                await smtpClient.SendMailAsync(message);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
