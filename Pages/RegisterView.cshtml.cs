using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using RegistrationSystem.Models;
using RegistrationSystem.DatabaseService;
using System;
using System.ComponentModel.DataAnnotations;



namespace RegistrationSystem.Pages
{
    public class RegisterViewModel : PageModel
    {
        public List<Module> ModuleList = new List<Module>();

        public List<string> courseRegistered = new List<string>();

        [BindProperty]
        public string searchquery { get; set; } = default!;

        [BindProperty]
        public string selected { get; set; }= default!;

        [BindProperty]
        public string deregister { get; set; } = default!;

        private void QueryGET()
        {
            string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                
                string queryAllDataSql = @"
                SELECT ID, ModuleCode, ModuleName, ModuleDetails, NumRegistered
                FROM Modules";

                using (SqlCommand command = new SqlCommand(queryAllDataSql, connection))
                {
                
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           
                            int value1 = reader.GetInt32(0);
                            string value2 = reader.GetString(1);  
                            string value3 = reader.GetString(2);
                            string value4 = reader.GetString(3);
                            int value5 = reader.GetInt32(4);
                            Module module = new Module(value1,value2,value3,value4,value5);
                            ModuleList.Add(module);
                            
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
                                        if (temp[i]!="")
                                        {
                                            courseRegistered.Add(temp[i]);
                                        }
                                        

                                    }
                                }
                               


                            }
                        }

                    }

                }catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }

        private void updateStudentsModule(int studentid, string update)
        {
            string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();


                    int primaryKeyValue = studentid;


                    string updateElementSql = @"
                UPDATE Students
                SET modulesRegistered = @NewValue1
                WHERE ID = @PrimaryKeyValue";

                    using (SqlCommand command = new SqlCommand(updateElementSql, connection))
                    {
                        // Set parameter values
                        command.Parameters.AddWithValue("@NewValue1", update);
                        command.Parameters.AddWithValue("@PrimaryKeyValue", primaryKeyValue);


                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Successfully Updated");
                           
                        }
                        else
                        {
                           Console.WriteLine("NOT UPDATED");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                   
                }
            }


        }

        private bool updateStudent(int studentid, string module)
        {
            string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                
                    int primaryKeyValue = studentid;

                   
                    string updateElementSql = @"
                UPDATE Students
                SET modulesRegistered = modulesRegistered + @NewValue1
                WHERE ID = @PrimaryKeyValue";

                    using (SqlCommand command = new SqlCommand(updateElementSql, connection))
                    {
                      
                        command.Parameters.AddWithValue("@NewValue1", ":"+module);
                        command.Parameters.AddWithValue("@PrimaryKeyValue", primaryKeyValue);

                       
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
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }


        }


        private void deregisterSingleModule(string modulecode)
        {
            try
            {
                string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";
                SqlConnection connection = new SqlConnection(connectionString);

                string updateModuleQuery = @"UPDATE Modules SET NumRegistered = NumRegistered - @NewValue1 WHERE ModuleCode = @PrimaryKeyValue";

                connection.Open();

                using (SqlCommand command = new SqlCommand(updateModuleQuery, connection))
                {
                    // Set parameter values
                    command.Parameters.AddWithValue("@NewValue1", 1);
                    command.Parameters.AddWithValue("@PrimaryKeyValue", modulecode);

                    // Execute the command
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Success");

                    }
                    else
                    {
                        Console.WriteLine("FAIL");

                    }
                }



            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }






        public IActionResult OnPost()
        {
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
                    }
                    QueryModulesRegistered(usr2);
                    courseRegistered.Remove(deregister);
                    string update = "";
                    for (int i = 0; i < courseRegistered.Count; i++)
                    {
                        update = update + ":" + courseRegistered[i];
                    }
                    update = update.Substring(1);
                    Console.WriteLine(update);
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
                }
                QueryModulesRegistered(usr);


                if (courseRegistered.Contains(selected))
                {
                    return RedirectToPage("/Privacy");
                }

                bool stst = updateStudent(usr,selected);
                
                QueryService queryService = new QueryService();
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
            }
            QueryGET();
            QueryModulesRegistered(usr);
        }


    }
}
