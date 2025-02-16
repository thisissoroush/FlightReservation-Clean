using System.Net;
using System.Net.Mail;
using FlightReservation.Application.Interfaces.Services;
using FlightReservation.Infrastructure.Email.Services.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace FlightReservation.Infrastructure.Email.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        
        var smtpClient = new SmtpClient
        {
            Host = _emailSettings.SmtpHost,
            Port = _emailSettings.SmtpPort,
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _emailSettings.Username,
                _emailSettings.Password)
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailSettings.From),
            Subject = subject,
            Body = body,
            IsBodyHtml = true,
        };

        mailMessage.To.Add(to);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
