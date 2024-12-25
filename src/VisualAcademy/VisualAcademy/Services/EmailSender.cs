
namespace VisualAcademy.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            Console.WriteLine(htmlMessage);
            return Task.CompletedTask;
        }
    }
}
