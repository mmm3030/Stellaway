using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Stellaway.Common.Settings;

namespace Stellaway.Services;

public class EmailSender(IOptions<MailSettings> mailSettings) : IEmailSender
{
    private readonly MailSettings _mailSettings = mailSettings.Value;

    public async Task SendEmailAsync(
        string email,
        string subject,
        string htmlMessage,
        byte[] qrCodeBytes,
        CancellationToken cancellationToken = default)
    {
        SmtpClient client = new SmtpClient
        {
            Port = _mailSettings.Port,
            Host = _mailSettings.Host,
            EnableSsl = _mailSettings.EnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = _mailSettings.UseDefaultCredentials,
            Credentials = new NetworkCredential(_mailSettings.Username, _mailSettings.Password)
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_mailSettings.From),
            Subject = subject,
            Body = htmlMessage,
            IsBodyHtml = _mailSettings.IsBodyHtml,
        };

        MemoryStream ms = new MemoryStream(qrCodeBytes);
        var attachment = new Attachment(ms, "qrcode.png");

        mailMessage.Attachments.Add(attachment);
        mailMessage.To.Add(email);

        await client.SendMailAsync(mailMessage, cancellationToken);
    }
}