using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using TheRaven.MVC.Areas.Identity.Data;

namespace TheRaven.MVC.Core.ForEmail;

public class EmailService : IEmailService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public EmailService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task SendEmailAsync(string email)
    {
        var user = await _userManager.FindByNameAsync(email);
        string Subject = "Register Confirmation Page";
        string htmlMessage = $"https://localhost:7292/VerificationEmail/{user.ConcurrencyStamp}?";

        try
        {
            var emailTosend = new MimeMessage();
            emailTosend.From.Add(MailboxAddress.Parse("tmehmet48.12@gmail.com"));
            emailTosend.To.Add(MailboxAddress.Parse(email));
            emailTosend.Subject = Subject;
            emailTosend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

            //send email

            using var client = new SmtpClient();//simple mail transfer protocol
            client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            client.Authenticate("tmehmet48.12@gmail.com", "lmwhrlzofytpnagm");
            client.Send(emailTosend);
            client.Disconnect(true);
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }
}
