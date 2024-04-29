using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using RegistrationSystem.Models;
using RegistrationSystem.Security;

namespace RegistrationSystem.Pages
{
    public class StudentViewModel : PageModel
    {
        public Student studentUser { get; set; } = default!;

        public int studentId { get; set; }

        public List<string> courseRegistered = new List<string>();

        private Student GetStudent(int student_id)
        {
            string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";
            EncryptionService encryptionService = new EncryptionService();


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
                            string value1 = encryptionService.Decrypt(reader.GetString(0));
                            string value2 = reader.GetString(1);
                            string value3 = encryptionService.Decrypt(reader.GetString(2));
                            string value4 = encryptionService.Decrypt(reader.GetString(3));

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

        private void QueryModulesRegistered(int studentid)
        {
            string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    int primaryKeyValue = studentid;

                    string queryAllDataSql = @"
                SELECT modulesRegistered
                FROM Students WHERE ID = @PrimaryKeyValue";

                    using (SqlCommand command = new SqlCommand(queryAllDataSql, connection))
                    {
                        command.Parameters.AddWithValue("@PrimaryKeyValue", primaryKeyValue);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string value1 = reader.GetString(0);
                                if (value1!="")
                                {
                                    Console.WriteLine(value1);
                                    List<string> temp = value1.Split(':').ToList();
                                    courseRegistered.Clear();
                                    for (int i = 0; i < temp.Count; i++)
                                    {
                                        if (temp[i] != "")
                                        {
                                            courseRegistered.Add(temp[i]);
                                        }


                                    }
                                }
                                
                                
                               


                            }
                        }

                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }



        public void OnGet(int id)
        {
            studentId = id;
            studentUser = GetStudent(id);
            QueryModulesRegistered(id);
        }
    }
}
