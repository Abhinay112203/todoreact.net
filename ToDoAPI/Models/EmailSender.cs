
using System.Net;
using System.Net.Mail;

namespace ToDoAPI.Models
{
    public class EmailSender: IEmailSender
    {
        private readonly IConfiguration _config;
        public EmailSender(IConfiguration configuration) {
            _config = configuration;
        }
        public void SendEmail(string email, string subject, string body)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com",587);
            client.EnableSsl = false;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_config["MAIL:email"], _config["MAIL:password"]);
            
            //Mail Creation
            MailMessage message = new MailMessage();
            message.From = new MailAddress(_config["MAIL:email"]);
            message.To.Add(email);
            message.Subject = subject;
            message.Body = body;
            client.Send(message);
        }
    }
    public interface IEmailSender
    {
        void SendEmail(string email, string subject, string body);
    }
}
