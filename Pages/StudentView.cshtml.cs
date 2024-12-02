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
            string connectionAuth = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";
            EncryptionService encryptionService = new EncryptionService();

            try
            {

                SqlConnection connection = new SqlConnection(connectionAuth);
                
                    connection.Open();

                    int primaryKey = student_id;

                    
                    string queryStudent = @"SELECT username, studentpassword, StudentName, course FROM Students WHERE ID = @PrimaryKey";

                SqlCommand command = new SqlCommand(queryStudent, connection);
                    
                       
                        command.Parameters.AddWithValue("@PrimaryKey", primaryKey);

                       
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                
                                string value1 = encryptionService.Decrypt(reader.GetString(0));
                                string value2 = reader.GetString(1);
                                string value3 = encryptionService.Decrypt(reader.GetString(2));
                                string value4 = encryptionService.Decrypt(reader.GetString(3));

                                return new Student(value1, value2, student_id, value3, value4);

                            }
                            else
                            {

                                return new Student("", "", 0, "", "");
                            }
                        }


                    

                
            } catch (Exception e) {

                
                return new Student("", "", 0, "", "");

            }

        }

        private void QueryModulesRegistered(int studentid)
        {
            string connectionAuth = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

            SqlConnection connection = new SqlConnection(connectionAuth);
            
                try
                {
                    connection.Open();
                    int primaryKey = studentid;

                    string queryRegistered = @"SELECT modulesRegistered FROM Students WHERE ID = @PrimaryKey";

                SqlCommand command = new SqlCommand(queryRegistered, connection);
                    
                        command.Parameters.AddWithValue("@PrimaryKey", primaryKey);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string value1 = reader.GetString(0);
                                if (value1!="")
                                {
                                    
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
                catch (Exception)
                {
                   
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
