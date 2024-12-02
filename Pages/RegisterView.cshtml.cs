using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using RegistrationSystem.Models;
using RegistrationSystem.DatabaseService;
using RegistrationSystem.Security;
using System;
using System.ComponentModel.DataAnnotations;



namespace RegistrationSystem.Pages
{
    public class RegisterViewModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public List<Module> ModuleList = new List<Module>();

        public List<string> courseRegistered = new List<string>();

        public int studentid;

        [BindProperty]
        public string searchquery { get; set; } = default!;

        [BindProperty]
        public string selected { get; set; }= default!;

        [BindProperty]
        public string deregister { get; set; } = default!;

        public RegisterViewModel(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        private string GetModuleCourse(string modulecode)
        {
            string connectionAuth = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";
            EncryptionService encryptionService = new EncryptionService();

            try
            {
             

                SqlConnection connection = new SqlConnection(connectionAuth);

                connection.Open();

                string primaryKey = modulecode;


                string queryStudent = @"
                SELECT Course
                FROM Modules
                WHERE ModuleCode = @PrimaryKey";

                SqlCommand command = new SqlCommand(queryStudent, connection);
                

                    command.Parameters.AddWithValue("@PrimaryKey", primaryKey);


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            
                              string value2 = reader.GetString(0);


                            return value2;

                        }
                        else
                        {

                            return "";
                        }
                    }


                


            }
            catch (Exception ex)
            {

                
                return "";

            }

        }

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


                


            }
            catch (Exception)
            {

               
                return new Student("", "", 0, "", "");

            }

        }


        private void QueryGET()
        {
            try
            {
                string connectionAuth = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

                SqlConnection connection = new SqlConnection(connectionAuth);
                
                    connection.Open();


                    string queryModules = @"SELECT ID, ModuleCode, ModuleName, ModuleDetails, NumRegistered, Course FROM Modules";

                SqlCommand command = new SqlCommand(queryModules, connection);
                    

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                int value1 = reader.GetInt32(0);
                                string value2 = reader.GetString(1);
                                string value3 = reader.GetString(2);
                                string value4 = reader.GetString(3);
                                int value5 = reader.GetInt32(4);
                                string value6 = reader.GetString(5);
                                Module module = new Module(value1, value2, value3, value4, value5, value6);
                                ModuleList.Add(module);

                            }
                        }

                    
                
            }catch (Exception)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                Logging logging = new Logging(webRootPath);
                logging.Logger("USER","CONNECTION","TIMEOUT");

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

                    string queryModules = @"SELECT modulesRegistered FROM Students WHERE ID = @PrimaryKey";

                    SqlCommand command = new SqlCommand(queryModules, connection);
                    
                        command.Parameters.AddWithValue("@PrimaryKey", primaryKey);
                        
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
                                        if (temp[i]!="")
                                        {
                                            courseRegistered.Add(temp[i]);
                                        }
                                        

                                    }
                                }
                               


                            }
                        }

                    

                }catch (Exception)
                {
                string webRootPath = _webHostEnvironment.WebRootPath;
                Logging logging = new Logging(webRootPath);
                    logging.Logger("USER", "CONNECTION", "TIMEOUT");
                }
            

        }

        private void updateStudentsModule(int studentid, string update)
        {
            string connectionAuth = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

            SqlConnection connection = new SqlConnection(connectionAuth);
            string webRootPath = _webHostEnvironment.WebRootPath;
            Logging logging = new Logging(webRootPath);
                try
                {
                    connection.Open();


                    int primaryKeyValue = studentid;


                    string queryStudentModule = @"UPDATE Students SET modulesRegistered = @NewRegCount WHERE ID = @PrimaryKey";

                    using (SqlCommand command = new SqlCommand(queryStudentModule, connection))
                    {
                       
                        command.Parameters.AddWithValue("@NewRegCount", update);
                        command.Parameters.AddWithValue("@PrimaryKey", primaryKeyValue);


                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            
                            logging.Logger(studentid.ToString(),"UPDATE MODULES","SUCCESS");
                           
                        }
                        else
                        {
                            logging.Logger(studentid.ToString(), "UPDATE MODULES", "FAIL");
                        }
                    }
                }
                catch (Exception)
                {
                    logging.Logger("USER", "CONNECTION", "TIMEOUT");

                }
            


        }

        private bool updateStudent(int studentid, string module)
        {
            string connectionAuth = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

            SqlConnection connection = new SqlConnection(connectionAuth);
            
                try
                {
                    connection.Open();

                
                    int primaryKey = studentid;

                   
                    string queryUpdateStudents = @"UPDATE Students SET modulesRegistered = modulesRegistered + @ModuleNew WHERE ID = @PrimaryKey";

                SqlCommand command = new SqlCommand(queryUpdateStudents, connection);
                    
                      
                        command.Parameters.AddWithValue("@ModuleNew", ":"+module);
                        command.Parameters.AddWithValue("@PrimaryKey", primaryKey);

                       
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
                string webRootPath = _webHostEnvironment.WebRootPath;
                Logging logging = new Logging(webRootPath);
                    logging.Logger("USER", "CONNECTION", "TIMEOUT");
                    return false;
                }
            


        }


        private void deregisterSingleModule(string modulecode)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            Logging logging = new Logging(webRootPath);
            try
            {
                string connectionAuth = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";
                SqlConnection connection = new SqlConnection(connectionAuth);

                string updateModuleQuery = @"UPDATE Modules SET NumRegistered = NumRegistered - @mCount WHERE ModuleCode = @PrimaryKey";

                connection.Open();

                SqlCommand command = new SqlCommand(updateModuleQuery, connection);
                
                   
                    command.Parameters.AddWithValue("@mCount", 1);
                    command.Parameters.AddWithValue("@PrimaryKey", modulecode);

                    
                    int rowsAffected = command.ExecuteNonQuery();

                    
                    
                



            }
            catch (Exception)
            {
                logging.Logger("USER","CONNECTION","TIMEOUT");
            }
        }






        public IActionResult OnPost()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            Logging logging = new Logging(webRootPath); 
            if (searchquery == null)
            {
                if (selected == null)
                {
                    if (deregister == null)
                    {
                        return RedirectToPage("/Privacy");
                    }

                    int usr2 = 0;
                    string? param2 = Request.Query["User"];
                    if (param2 != null)
                    {
                        usr2 = Int32.Parse(param2);
                        studentid = usr2;
                    }
                    QueryModulesRegistered(usr2);
                    courseRegistered.Remove(deregister);
                    string update = "";
                    for (int i = 0; i < courseRegistered.Count; i++)
                    {
                        update = update + ":" + courseRegistered[i];
                    }
                    update = update.Substring(1);
                    logging.Logger(usr2.ToString(),"DEREGISTER MODULE","SUCCESS");
                    updateStudentsModule(usr2, update);
                    deregisterSingleModule(deregister);
                    QueryModulesRegistered(usr2);
                    QueryGET();

                    return Page();
                }
                int usr = 0;
                string? param1 = Request.Query["User"];
                if (param1 != null)
                {
                    usr = Int32.Parse(param1);
                    studentid = usr;
                }
                QueryModulesRegistered(usr);


                if (courseRegistered.Contains(selected))
                {
                    return RedirectToPage("/InvalidReg");
                }

                Student checkstudent = GetStudent(usr);
                string checkmodule = GetModuleCourse(selected);

             
               
                if (checkstudent.Course != checkmodule)
                {
                    return RedirectToPage("/InvalidCourse");

                }
                logging.Logger(usr.ToString(),"REGISTER MODULE","SUCCESS");
                bool stst = updateStudent(usr,selected);
                
                QueryService queryService = new QueryService(webRootPath);
                queryService.updateModule(selected);
                QueryModulesRegistered(usr);
                QueryGET();
                



                return Page();
            }
            QueryGET();
            List<Module> modules = new List<Module>();
            modules = ModuleList;
            List<Module> searchresult = new List<Module>();
            for (int i = 0; i < modules.Count; i++)
            {
                string? book_in_library = modules[i].ModuleCode;
                if (book_in_library?.ToLower().IndexOf(searchquery.ToLower()) != -1)
                {
                    searchresult.Add(modules[i]);
                }
            }

            ModuleList = searchresult;
            
            

            return Page();
        }
        public void OnGet()
        {
            int usr = 0;
            string? param1 = Request.Query["User"];
            if (param1 != null)
            {
                usr = Int32.Parse(param1);
                studentid = usr;
            }
            QueryGET();
            QueryModulesRegistered(usr);
        }


    }
}
