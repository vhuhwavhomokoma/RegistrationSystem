using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace RegistrationSystem.Pages
{
    public class FinalizeChangeModel : PageModel
    {
        [BindProperty]
        public string newpassword { get; set; } = default!;

        [BindProperty]
        public string confirmpassword { get; set; } = default!;

        public static string Username { get; set; } = default!;

        private bool updateAdminPassword()
        {
            string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Specify the primary key value or any condition to identify the element to update


                    // SQL command to update the element
                    string updateElementSql = @"
                UPDATE Administrators
                SET adminpassword = @NewValue1
                WHERE username = @PrimaryKeyValue";

                    using (SqlCommand command = new SqlCommand(updateElementSql, connection))
                    {
                        // Set parameter values
                        Console.WriteLine(Username);
                        command.Parameters.AddWithValue("@NewValue1", newpassword);
                        command.Parameters.AddWithValue("@PrimaryKeyValue", Username);

                       
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
            }


        }

        private bool updatePassword()
        {
            string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Specify the primary key value or any condition to identify the element to update
                    

                    // SQL command to update the element
                    string updateElementSql = @"
                UPDATE Students
                SET studentpassword = @NewValue1
                WHERE username = @PrimaryKeyValue";

                    using (SqlCommand command = new SqlCommand(updateElementSql, connection))
                    {
                        // Set parameter values
                        Console.WriteLine(Username);
                        command.Parameters.AddWithValue("@NewValue1", newpassword);
                        command.Parameters.AddWithValue("@PrimaryKeyValue", Username);

                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Update successful
                            connection.Close();
                            return true;
                        }
                        else
                        {
                            // No matching element found for the update
                            
                            connection.Close();
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
            }


        }


        public IActionResult OnPost()
        {
            
            if (newpassword == null || confirmpassword == null )
            {
                return Page();
            }
        
            if (newpassword == confirmpassword)
            {
                
                if (Username.Substring(0,1)=="u") {
                    bool status2 = updatePassword();
                    
                    if (status2)
                    {
                        return RedirectToPage("/Index");
                    }
                    return Page();
                }
                bool status = updateAdminPassword();
                
                if (status)
                {
                    return RedirectToPage("/Index");
                }
                return Page();
                
            }

            return Page();
        }


        public void OnGet(string usr)
        {
            Username = usr;
            
        }
    }
}
