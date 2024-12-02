using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using RegistrationSystem.Security;

namespace RegistrationSystem.Pages
{
    public class FinalizeChangeModel : PageModel
    {
        [BindProperty]
        public string newpassword { get; set; } = default!;

        [BindProperty]
        public string confirmpassword { get; set; } = default!;

        [BindProperty]
        public string verificationCode { get; set; } = default!;

        public static string Username { get; set; } = default!;

        private static string code { get; set; } = default!;

        private bool updateAdminPassword()
        {
            string connectionAuth = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";
            EncryptionService encryptionService = new EncryptionService();
            SqlConnection connection = new SqlConnection(connectionAuth);
            
                try
                {
                    connection.Open();

                   
                    string queryUpdateAdminPass = @"UPDATE Administrators SET adminpassword = @NewPass WHERE username = @PrimaryKey";

                SqlCommand command = new SqlCommand(queryUpdateAdminPass, connection);
                    
                        
                       
                        command.Parameters.AddWithValue("@NewPass", encryptionService.Encrypt(newpassword));
                        command.Parameters.AddWithValue("@PrimaryKey", encryptionService.Encrypt(Username));

                       
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            
                            connection.Close();
                            return true;
                        }
                        else
                        {
                    

                            connection.Close();
                            return false;
                        }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
            


        }

        private bool updatePassword()
        {
            string connectionAuth = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

            SqlConnection connection = new SqlConnection(connectionAuth);
            
                try
                {
                    EncryptionService encryptionService = new EncryptionService();
                    
                    connection.Open();

                    string updateStudentPass = @"UPDATE Students SET studentpassword = @NewPass WHERE username = @PrimaryKey";

                SqlCommand command = new SqlCommand(updateStudentPass, connection);
                    
                        
                        command.Parameters.AddWithValue("@NewPass", encryptionService.Encrypt(newpassword));
                        command.Parameters.AddWithValue("@PrimaryKey", encryptionService.Encrypt(Username));

                        
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                          
                            connection.Close();
                            
                            return true;
                        }
                        else
                        {
                            
                            
                            connection.Close();
                           
                            return false;
                        }
                    
                }
                catch (Exception)
                {
                    
                    return false;
                }
            


        }


        public IActionResult OnPost()
        {
            
            if (newpassword == null || confirmpassword == null  || verificationCode == null)
            {
                return Page();
            }
        
            if (newpassword == confirmpassword)
            {
                
                if (Username.Substring(0,1)=="u") {
                   
                    if (verificationCode==code)
                    {
                        
                        bool status2 = updatePassword();

                        if (status2)
                        {
                            return RedirectToPage("/Index");
                        }
                        return Page();
                    }
                    return Page();
                    
                }
                //Verify Admin code
                if (verificationCode==code) {
                    bool status = updateAdminPassword();

                    if (status)
                    {
                        return RedirectToPage("/Index");
                    }
                    return Page();

                }

                return Page();
            }

            return Page();
        }


        public void OnGet(string usr,string cd)
        {
            Username = usr;
            code = cd;
            
        }
    }
}
