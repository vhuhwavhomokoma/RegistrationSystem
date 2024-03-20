using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using RegistrationSystem.Models;

namespace RegistrationSystem.Pages
{
    public class StudentViewModel : PageModel
    {
        public Student studentUser { get; set; } = default!;

        private Student GetStudent(int student_id)
        {
            string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                int primaryKeyValue = student_id;

                // SQL command to query the specific element
                string queryElementSql = @"
                SELECT username, studentpassword, StudentName, course
                FROM Students
                WHERE ID = @PrimaryKeyValue";

                using (SqlCommand command = new SqlCommand(queryElementSql, connection))
                {
                    // Set parameter value
                    command.Parameters.AddWithValue("@PrimaryKeyValue", primaryKeyValue);

                    // Execute the command and read the result
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Access data using reader
                            string value1 = reader.GetString(0);
                            string value2 = reader.GetString(1);
                            string value3 = reader.GetString(2);
                            string value4 = reader.GetString(3);

                            return new Student(value1,value2,student_id,value3,value4);

                        }
                        else
                        {
                            
                            return new Student("", "", 0, "", "");
                        }
                    }


                }

            }

        }



        public void OnGet(int id)
        {
            studentUser = GetStudent(id);
        }
    }
}
