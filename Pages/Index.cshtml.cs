using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using RegistrationSystem.Models;
using System.Xml.Linq;
using RegistrationSystem.Security;
using RegistrationSystem.Supportfeatures;


namespace RegistrationSystem.Pages
{
    public class IndexModel : PageModel
    {

        [BindProperty]
        public string usrnm { get; set; } = default!;
        [BindProperty]
        public string pw { get; set; } = default!;

        [BindProperty]
        public string verify { get; set; } = default!;

        [BindProperty]
        public string connection { get; set; } = default!;

        public List<Student> StudentList = new List<Student>();

        public List<Administrator> AdministratorList = new List<Administrator>();

        /*
         * This is for querying all the student entries to create a student list for use to authenticate student user access
         */

        private void QueryGET()
        {
            string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=45;";
            //initialise encryption service to encrypt or decrypt student data
            EncryptionService encryptionService = new EncryptionService();

            SqlConnection connection = new SqlConnection(connectionString);
            
                try
                {
                    connection.Open();

                   
                    string queryStudent = @"SELECT username, studentpassword, ID, StudentName, course FROM Students";

                    using (SqlCommand command = new SqlCommand(queryStudent, connection))
                    {
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //Decrypt data in order for use in the system
                                
                                string value1 = encryptionService.Decrypt(reader.GetString(0));
                                string value2 = encryptionService.Decrypt(reader.GetString(1));
                                Console.WriteLine(value1);
                                int value3 = reader.GetInt32(2);
                                string value4 = encryptionService.Decrypt(reader.GetString(3));
                                string value5 = encryptionService.Decrypt(reader.GetString(4));
                                
                                //Create and add a student to the Student list
                                Student student = new Student(value1, value2, value3, value4, value5);
                                StudentList.Add(student);

                            }
                        }

                    }
                }
                catch (Exception)
                {
                    Logging logging = new Logging();
                    logging.Logger(usrnm,"CONNECTION","TIMEOUT");
                    Page();
                }
            

        }

        /*
        * This is for querying all the administrator entries to create a administrator list for use to authenticate administrator user access
        */

        private void QueryAdmin()
        {
            string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=45;";

            
                try
                {

                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();

                    
                    string queryAdministrator = @"SELECT ID, username, adminpassword FROM Administrators";

                    using (SqlCommand command = new SqlCommand(queryAdministrator, connection))
                    {
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //Decrypt data in order for use in the system
                                int value3 = reader.GetInt32(0);
                                string value4 = reader.GetString(1);
                                string value5 = reader.GetString(2);
                               //Create and add a administrator to the Administrator list
                                 Administrator admin = new Administrator(value4, value5, value3);
                                AdministratorList.Add(admin);

                            }
                        }

                    }
                }
                catch (Exception)
                {
                Logging logging = new Logging();
                logging.Logger(usrnm,"CONNECTION","TIMEOUT");
                    Page();
                }
            

        }



        public IActionResult OnPost()
        {
            verify = "CHECK";
            Logging logging = new Logging();

            if (pw == null || usrnm == null) //Ensure that both username and password are filled before authenticating
            {
                return Page();
            }
            //Processing Administrator authentication
            
            if (usrnm.Substring(0, 1) == "a")
            {
                
                QueryAdmin();
                
                List<Administrator> admins = AdministratorList;
                
                for (int i = 0; i < admins.Count; i++)
                {
                    if (usrnm == admins[i].UserName && pw == admins[i].Password)
                    {
                        logging.Logger(usrnm,"ADMIN LOG IN","SUCCESS"); 
                        Support support = new Support();
                        string code = support.randomCode();
                        Authentication authentication = new Authentication();
                        string emailtext = $"Greetings,\r\n\r\n We have received an Administrator login request. To proceed with the login, please use the following verification code:\r\n\r\nVerification Code: {code}\r\n\r\nPlease enter this code on the verification page to confirm your identity and proceed with logging in. If you did not request this please contact our support team immediately for assistance. ";
                        authentication.Email(emailtext, "vhuhwavhomokoma@gmail.com", "VERIFY LOGIN");
                        return RedirectToPage("/AdminAuthentication", new { cd = code });

                    }
                }
                verify = "INCORRECT";
                logging.Logger(usrnm,"ADMIN LOG IN","FAIL");
                return Page();
                }
                QueryGET();
                //Processing Student authentication
                List<Student> students = StudentList;
            

            for (int i = 0; i < students.Count; i++)
                {
                
                    if (usrnm == students[i].UserName && pw == students[i].Password)
                    {
                    logging.Logger(usrnm,"USER LOG IN","SUCCESS");
                        return RedirectToPage("/StudentView", new { id = students[i].StudentId });
                    }
                }
            verify = "INCORRECT";
            logging.Logger(usrnm,"USER LOG IN","FAIL");
            Monitoring monitoring = new Monitoring();
            monitoring.MonitorUSERAcvitiy();

                return Page();

            }

            public void OnGet()
            {

            }
        }
    }
