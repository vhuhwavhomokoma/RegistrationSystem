
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;

namespace RegistrationSystem.Security
{
    public class Authentication
    {
        public Authentication() { }

        public void Email(string emailtext, string receiver) {
            try
            {
                string senderEmail = "v55218585@gmail.com";
                string appPassword = "cuki xajf jxbv tfdd";

                // Create a MailMessage object
                MailMessage message = new MailMessage();
                message.From = new MailAddress(senderEmail);
                message.To.Add(new MailAddress("vhuhwavhomokoma@gmail.com"));
                message.Subject = "Test Email";
                message.Body = "This is a test email sent using System.Net.Mail with App Password.";

                // Configure SMTP client
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(senderEmail, appPassword);

                // Send the email
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
