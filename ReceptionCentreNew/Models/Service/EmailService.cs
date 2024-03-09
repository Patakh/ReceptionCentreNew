using Microsoft.AspNetCore.Identity.UI.Services;

namespace ReceptionCentreNew.Models.Service;

public class EmailService : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message) =>
         Task.CompletedTask;
}