using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RegistrationSystem.Models;
using RegistrationSystem.Security;
using RegistrationSystem.Supportfeatures;
using System.Data.SqlClient;

namespace RegistrationSystem.Pages
{
    public class ChangePasswordViewModel : PageModel
    {
        [BindProperty]
        public string enteredUsername { get; set; } = default!;

        private string _email;

        private void queryAdminEmail()
        {

        }


        private void queryEmail(string username)
        {
            try { 
            string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";
            //initialise encryption service to encrypt or decrypt student data
            EncryptionService encryptionService = new EncryptionService();

            SqlConnection connection = new SqlConnection(connectionString);

          
                connection.Open();

                
                string queryStudent = @"SELECT email_address FROM Students WHERE username = @Value1";

                using (SqlCommand command = new SqlCommand(queryStudent, connection))
                {
                    command.Parameters.AddWithValue("@Value1",encryptionService.Encrypt(username));

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Decrypt data in order for use in the system
                            _email = encryptionService.Decrypt(reader.GetString(0));
                            
                   
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                
            }

        }

        public IActionResult OnPost()
        {
            if (enteredUsername==null)
            {
                
                return Page();
            }
           
            queryEmail(enteredUsername);
            Authentication authentication = new Authentication();
            Support support = new Support();
            string code = support.randomCode();
            

            string emailtext = $"Greetings,\r\n\r\n We have received a request to change the password associated with your account. To proceed with this change, please use the following verification code:\r\n\r\nVerification Code: {code}\r\n\r\nPlease enter this code on the password change page to confirm your identity and proceed with updating your password. If you did not request this change or have any concerns, please contact our support team immediately for assistance. ";
            authentication.Email(emailtext,_email,"CHANGE PASSWORD CODE");
        

            return RedirectToPage("/FinalizeChange", new { usr = enteredUsername, cd = code });
        }

        public void OnGet()
        {
        }
    }
}
