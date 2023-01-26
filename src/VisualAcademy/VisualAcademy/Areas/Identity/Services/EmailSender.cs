using Microsoft.AspNetCore.Identity.UI.Services;

namespace VisualAcademy.Areas.Identity.Services
{
    // ASP.NET Core Identity 인증과 권한 
    // Abstractions: Interfaces => IEmailSender
    // Implementations: Classes => EmailSender, SendGridEmailSender, ...
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(
            string email,
            string subject,
            string htmlMessage)
        {
            // 여기 코드는
            // 직접 SMTP 서버를 구성하거나,
            // SendGrid와 같은 클라우드 서비스를 사용하면 됩니다.
            // 다만, 학습 차원에서는 여기에 중단점 설정 후 htmlMessage를 보고 Confirm 절차를 진행해도 됩니다.
            return Task.CompletedTask;
        }
    }
}
