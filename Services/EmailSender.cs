using Microsoft.AspNetCore.Identity.UI.Services;

namespace DodjelaStanovaZG.Services;

public class EmailSender(ILogger<EmailSender> logger) : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        logger.LogInformation("Sending email to {Email} with subject '{Subject}'", email, subject);
        return Task.CompletedTask;
    }
}