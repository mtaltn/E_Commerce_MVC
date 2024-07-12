namespace TheRaven.MVC.Core.ForEmail;

public interface IEmailService
{
    Task SendEmailAsync(string email);
}
