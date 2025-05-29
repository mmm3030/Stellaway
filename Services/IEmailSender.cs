namespace Stellaway.Services;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string htmlMessage, byte[] qrCodeBytes, CancellationToken cancellationToken = default);
}