
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Net;

namespace RegistrationSystem.Security
{
    public class Authentication
    {
        public Authentication() { }

        public void Email(string emailtext, string receiver, string emailsubject) {
            try
            {
                string sender = "v55218585@gmail.com";
                string Password = "cuki xajf jxbv tfdd";

                
                MailMessage emailmessage = new MailMessage();
                emailmessage.From = new MailAddress(sender);
                emailmessage.To.Add(new MailAddress(receiver));
                emailmessage.Subject = emailsubject;
                emailmessage.Body = emailtext;

               
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(sender, Password);

            
                smtp.Send(emailmessage);

                
            }
            catch (Exception)
            {
                
            }
        }


    }
}
