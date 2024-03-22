using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using RegistrationSystem.Models;
using System.Xml.Linq;

namespace RegistrationSystem.Pages
{
    public class IndexModel : PageModel
    {

        [BindProperty]
        public string usrnm { get; set; } = default!;
        [BindProperty]
        public string pw { get; set; } = default!;

        public List<Student> StudentList = new List<Student>();

        public List<Administrator> AdministratorList = new List<Administrator>();

        private void QueryGET()
        {
            string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // SQL command to check if specific values are present
                    string queryAllDataSql = @"
                SELECT username, studentpassword, ID, StudentName, course
                FROM Students";

                    using (SqlCommand command = new SqlCommand(queryAllDataSql, connection))
                    {
                        // Execute the command and read the result
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Access data using reader for each row
                                string value1 = reader.GetString(0);
                                string value2 = reader.GetString(1);  // Assuming Column2 is of type NVARCHAR
                                int value3 = reader.GetInt32(2);
                                string value4 = reader.GetString(3);
                                string value5 = reader.GetString(4);
                                Student student = new Student(value1, value2, value3, value4, value5);
                                StudentList.Add(student);

                            }
                        }

                    }
                }
                catch (Exception)
                {
                    Page();
                }
            }

        }

        private void QueryAdmin()
        {
            string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    
                    string queryAllDataSql = @"
                SELECT ID, username, adminpassword
                FROM Administrators";

                    using (SqlCommand command = new SqlCommand(queryAllDataSql, connection))
                    {
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                
                                int value3 = reader.GetInt32(0);
                                string value4 = reader.GetString(1);
                                string value5 = reader.GetString(2);
                                Administrator admin = new Administrator(value4, value5, value3);
                                AdministratorList.Add(admin);

                            }
                        }

                    }
                }
                catch (Exception)
                {
                    Page();
                }
            }

        }



        public IActionResult OnPost()
        {
            if (pw == null || usrnm == null)
            {
                return Page();
            }

            if (usrnm.Substring(0, 1) == "a")
            {
                QueryAdmin();
                List<Administrator> admins = AdministratorList;
                for (int i = 0; i < admins.Count; i++)
                {
                    if (usrnm == admins[i].UserName && pw == admins[i].Password)
                    {
                        return RedirectToPage("/AdministratorView");

                    }
                }

                    return Page();
                }
                QueryGET();
                List<Student> students = StudentList;
                for (int i = 0; i < students.Count; i++)
                {
                    if (usrnm == students[i].UserName && pw == students[i].Password)
                    {
                        return RedirectToPage("/StudentView", new { id = students[i].StudentId });
                    }
                }

                return Page();

            }

            public void OnGet()
            {

            }
        }
    }
