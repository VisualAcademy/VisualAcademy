using System.Threading.Tasks;

namespace All.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, bool isBodyHtml = true);
    }
}
