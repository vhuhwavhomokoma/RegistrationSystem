using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using RegistrationSystem.Models;

namespace RegistrationSystem.Pages
{
    public class RegisterViewModel : PageModel
    {
        private void QueryGET()
        {
            string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // SQL command to check if specific values are present
                string queryAllDataSql = @"
                SELECT ID, ModuleCode, ModuleName, ModuleDetails, NumRegistered
                FROM Modules";

                using (SqlCommand command = new SqlCommand(queryAllDataSql, connection))
                {
                    // Execute the command and read the result
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Access data using reader for each row
                            int value1 = reader.GetInt32(0);
                            string value2 = reader.GetString(1);  // Assuming Column2 is of type NVARCHAR
                            string value3 = reader.GetString(2);
                            string value4 = reader.GetString(3);
                            int value5 = reader.GetInt32(4);
                            
                        }
                    }

                }
            }

        }



        public void OnGet()
        {
        }
    }
}
