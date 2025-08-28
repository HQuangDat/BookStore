using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;
using System.Net.Mail;

namespace BookStore.Service
{
    public class SendMailService
    {
        public async Task SendEmail(string email, string resetLink)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(email));
            message.Subject = "Reset your account password!";
            message.Body = $"<p>Click <a href='{resetLink}'>here</a> to reset your password.</p>";
            message.IsBodyHtml = true;
            message.From = new MailAddress("hoquangdat123@gmail.com");
            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential("hoquangdat123@gmail.com", "vpkp ssdb coam qfxp");
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
            }
            Log.Information("Email sent successfully");
        }
    }
}
