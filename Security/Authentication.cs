
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;

namespace RegistrationSystem.Security
{
    public class Authentication
    {
        public Authentication() { }

        public void Email(string emailtext, string receiver, string subject) {
            try
            {
                string senderEmail = "v55218585@gmail.com";
                string appPassword = "cuki xajf jxbv tfdd";

                
                MailMessage message = new MailMessage();
                message.From = new MailAddress(senderEmail);
                message.To.Add(new MailAddress(receiver));
                message.Subject = subject;
                message.Body = emailtext;

               
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(senderEmail, appPassword);

            
                smtpClient.Send(message);

                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
            }
        }


    }
}
