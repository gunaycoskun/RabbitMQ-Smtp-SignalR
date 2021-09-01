using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRabbitMQ
{
    static class EMailSender
    {
        public static void Send(string to, string message)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;

            NetworkCredential credential = new NetworkCredential("test@gmail.com","********");
            smtpClient.Credentials = credential;
            MailAddress sender = new MailAddress("test@gmail.com", "Günay Coşkun");
            MailAddress alici = new MailAddress(to);

            MailMessage mail = new MailMessage(sender, alici);
            mail.Subject = "Subject";
            mail.Body = message;
            smtpClient.Send(mail);
        }
    }
}
