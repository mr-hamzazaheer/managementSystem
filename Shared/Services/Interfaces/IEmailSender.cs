
namespace Shared.Service.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string from, string to, string subject, string bodyHtml,List<string>? cc = null, List<string>? bcc = null, List<(byte[] Content, string FileName, string ContentType)>? attachments = null);
    }
}
