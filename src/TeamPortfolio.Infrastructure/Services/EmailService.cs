using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using TeamPortfolio.Application.Interfaces.Services;

namespace TeamPortfolio.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailConfirmationAsync(string email, string confirmationLink)
    {
        var body = $"""
            <div dir="rtl" style="font-family:Arial,sans-serif">
            <h2>تأیید آدرس ایمیل</h2>
            <p>برای تأیید ایمیل خود روی لینک زیر کلیک کنید:</p>
            <a href="{confirmationLink}">تأیید ایمیل</a>
            </div>
            """;
        await SendAsync(email, "تأیید آدرس ایمیل", body);
    }

    public async Task SendPasswordResetAsync(string email, string resetLink)
    {
        var body = $"""
            <div dir="rtl" style="font-family:Arial,sans-serif">
            <h2>بازیابی رمز عبور</h2>
            <p>برای بازیابی رمز عبور روی لینک زیر کلیک کنید:</p>
            <a href="{resetLink}">بازیابی رمز عبور</a>
            </div>
            """;
        await SendAsync(email, "بازیابی رمز عبور", body);
    }

    private async Task SendAsync(string to, string subject, string htmlBody)
    {
        var host = _configuration["Email:SmtpHost"] ?? "smtp.gmail.com";
        var port = int.Parse(_configuration["Email:SmtpPort"] ?? "587");
        var user = _configuration["Email:Username"] ?? "";
        var pass = _configuration["Email:Password"] ?? "";
        var fromName = _configuration["Email:FromName"] ?? "Team Portfolio";
        var fromEmail = _configuration["Email:FromEmail"] ?? user;

        var msg = new MimeMessage();
        msg.From.Add(new MailboxAddress(fromName, fromEmail));
        msg.To.Add(new MailboxAddress("", to));
        msg.Subject = subject;
        msg.Body = new TextPart("html") { Text = htmlBody };

        using var client = new SmtpClient();
        await client.ConnectAsync(host, port, SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(user, pass);
        await client.SendAsync(msg);
        await client.DisconnectAsync(true);
    }
}
