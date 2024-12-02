using RegistrationSystem.Models;
using System.Data.SqlClient;
using RegistrationSystem.Supportfeatures;
using RegistrationSystem.Security;

namespace RegistrationSystem.DatabaseService
{
    public class QueryService
    {
        string filePath;
        public QueryService(string rootpath) { 
        
            filePath = rootpath;
        }


        public string QueryAdminEmail()
        {
            try
            {
                string connectionAuth = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";
                
                EncryptionService encryptionService = new EncryptionService();

                SqlConnection connection = new SqlConnection(connectionAuth);

                string _email = "";
                connection.Open();


                string queryAdmin = @"SELECT emailaddress FROM Administrators WHERE ID = 1";

                SqlCommand command = new SqlCommand(queryAdmin, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Decrypt data in order for use in the system
                             _email = encryptionService.Decrypt(reader.GetString(0));


                        }
                    }

                    return _email;

                
            }
            catch (Exception)
            {
               
                return "";

            }

        }

        public List<Student> QueryGET()
        {

            string connectionAuth = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

            List<Student> students = new List<Student>();
            EncryptionService encryptionService = new EncryptionService();

            SqlConnection connection = new SqlConnection(connectionAuth);
            
                try
                {
                    connection.Open();

                    
                    string queryStudents = @"SELECT username, studentpassword, ID, StudentName, course FROM Students";

                SqlCommand command = new SqlCommand(queryStudents, connection);
                    
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                
                                string value1 = encryptionService.Decrypt(reader.GetString(0));
                                
                                string value2 = reader.GetString(1);  
                                int value3 = reader.GetInt32(2);
                                string value4 = encryptionService.Decrypt(reader.GetString(3));
                                string value5 = encryptionService.Decrypt(reader.GetString(4));
                                Student student = new Student(value1, value2, value3, value4, value5);
                                students.Add(student);

                            }

                            return students;


                        }



                    
                }
                catch (Exception)
                {
                    Logging logging = new Logging(filePath);
                    logging.Logger("USER","CONNECTION","TIMEOUT");

                    return students;
                }
            

        }

		public void updateModule(string module)
		{
            string connectionAuth = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

            SqlConnection connection = new SqlConnection(connectionAuth);
            
                Logging logging = new Logging(filePath);
                try
                {
                    connection.Open();

                    
                    string primaryKey = module;

                   
                    string queryUpdateModules = @"UPDATE Modules SET NumRegistered = NumRegistered + @upCount WHERE ModuleCode = @PrimaryKey";

                SqlCommand command = new SqlCommand(queryUpdateModules, connection);
                    
                        
                        command.Parameters.AddWithValue("@upCount", 1);
                        command.Parameters.AddWithValue("@PrimaryKey", primaryKey);

                       
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            
                            connection.Close();
                            
                        }
                        else
                        {
                          
                            connection.Close();
                            
                        }
                    
                }
                catch (Exception)
                {
                    logging.Logger("USER","CONNECTION","TIMEOUT");
                }
            

        }

		public List<Models.Module> QueryModule()
		{
            try
            {
                string connectionAuth = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

                List<Models.Module> modules = new List<Models.Module>();

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
                                Models.Module module = new Models.Module(value1, value2, value3, value4, value5, value6);
                                modules.Add(module);

                            }

                            return modules;

                        }

                    
                
            }catch (Exception)
            {
                
                return new List<Models.Module>();
            }

		}

		public void queryAddModule(string modulecode,string modulename,string moduledescription,string modulecourse)
		{
			string connectionAuth = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";


            SqlConnection connection = new SqlConnection(connectionAuth);
			
                Logging logging = new Logging(filePath);
                try
				{
                    
                    connection.Open();
					
					int lastIndex = 0;
					
					string QueryModule = @"SELECT ID FROM Modules WHERE ID=(SELECT max(ID) FROM Modules)";


					using (SqlCommand command = new SqlCommand(QueryModule, connection))
					{

						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								lastIndex = reader.GetInt32(0);
								

							}



						}
					}

					string InsertModule = @"INSERT INTO Modules(ID,ModuleCode,ModuleName,ModuleDetails,NumRegistered,Course) VALUES (@MIndex,@code,@name,@desc,@num,@modulecourse)";

					using (SqlCommand command = new SqlCommand(InsertModule, connection))
					{
						command.Parameters.AddWithValue("@MIndex", lastIndex + 1);
						command.Parameters.AddWithValue("@code", modulecode);
						command.Parameters.AddWithValue("@name", modulename);
						command.Parameters.AddWithValue("@desc", moduledescription);
						command.Parameters.AddWithValue("@num", 0);
                        command.Parameters.AddWithValue("modulecourse",modulecourse);
						

						int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0) {
                            logging.Logger("ADMIN","ADD NEW MODULE","SUCCESS"); 
                        
                        }else {
                            logging.Logger("ADMIN", "ADD NEW MODULE", "FAIL");

                        }


					}
				}
				catch (Exception)
				{
                    logging.Logger("ADMIN", "CONNECTION", "TIMEOUT");
                }
			



		}

		public void degregisterQuery(int studentid)
		{
            string connectionAuth = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";


            SqlConnection connection = new SqlConnection(connectionAuth);
            
                Logging logging = new Logging(filePath);
                try
                {
                    
                    connection.Open();

                    string AlterModules = @"SELECT modulesRegistered FROM Students WHERE ID = @student_id";
                    List<string> modulesReg = new List<string>();

                    using (SqlCommand command = new SqlCommand(AlterModules, connection))
                    {
                        command.Parameters.AddWithValue("@student_id", studentid);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                modulesReg = reader.GetString(0).Split(":").ToList();
                               
                            }


                        }

                    }

                    

                    string updateModuleQuery = @"UPDATE Modules SET NumRegistered = NumRegistered - @regnum WHERE ModuleCode = @PrimaryKey";

                    for (int i = 1; i < modulesReg.Count; i++ )
                    {
                        using (SqlCommand command = new SqlCommand(updateModuleQuery, connection))
                        {
                           
                            command.Parameters.AddWithValue("@regnum", 1);
                            command.Parameters.AddWithValue("@PrimaryKey", modulesReg[i]);

                           
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                
                                logging.Logger("ADMIN", "DEREGISTER STUDENT", "SUCCESS");

                            }
                            else
                            {
                                logging.Logger("ADMIN", "DEREGISTER STUDENT", "FAIL");

                            }
                        }



                    }


                    

                    string QueryDeregister = @"DELETE FROM Students WHERE ID = @StudentId";

                    using (SqlCommand command = new SqlCommand(QueryDeregister, connection))
                    {
                        command.Parameters.AddWithValue("@StudentId", studentid);

                        int rowsAffected = command.ExecuteNonQuery();
                    
                    }

                    

                }
                catch (Exception)
                {
                    logging.Logger("ADMIN", "CONNECTION", "TIMEOUT");
                }
            


        }


        public void removeQuery(int moduleid)
        {
            string connectionAuth = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";


            SqlConnection connection = new SqlConnection(connectionAuth);
             Logging logging = new Logging(filePath);

            try
                {
                    

                    connection.Open();



                    string QueryDeregister = @"DELETE FROM Modules WHERE ID = @m_id";

                    using (SqlCommand command = new SqlCommand(QueryDeregister, connection))
                    {
                        command.Parameters.AddWithValue("@m_id", moduleid);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            logging.Logger("ADMIN", "REMOVE MODULE", "SUCCESS");
                        }
                        else
                        {
                            logging.Logger("ADMIN", "REMOVE MODULE", "FAIL");
                        }


                    }
                }
                catch (Exception)
                {
                    logging.Logger("ADMIN", "CONNECTION", "TIMEOUT");
                    
                }
            


        }




        public void queryAddStudent(string fullname,string course,string email)
        {
			string connectionAuth = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";
            EncryptionService encryptionService = new EncryptionService();


            SqlConnection connection = new SqlConnection(connectionAuth);
			
                Logging logging = new Logging(filePath);
                try
				{
                    
                    connection.Open();
					Support support = new Support();
					int lastIndex = 0;
					string nextusername = "";
					string QueryStudent = @"SELECT ID, username FROM Students WHERE ID=(SELECT max(ID) FROM Students)";


					using (SqlCommand command = new SqlCommand(QueryStudent, connection))
					{
						
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								lastIndex = reader.GetInt32(0);
								string value2 = encryptionService.Decrypt(reader.GetString(1));
								nextusername = support.generateUsername(value2);
                                
                                



                            }



						}
					}

						string QueryInsert = @"INSERT INTO Students(ID,StudentName,username,studentpassword,course,modulesRegistered,email_address) VALUES (@indexvalue,@fname,@uname,@passw,@studentcourse,@mregistered,@mail)";

						using (SqlCommand command = new SqlCommand(QueryInsert, connection))
						{
                        string gen_pasword = support.RandomPassword();

                            command.Parameters.AddWithValue("@indexvalue", lastIndex+1);
							command.Parameters.AddWithValue("@fname", fullname);
							command.Parameters.AddWithValue("@uname", encryptionService.Encrypt(nextusername));
							command.Parameters.AddWithValue("@passw", encryptionService.Encrypt(gen_pasword));
							command.Parameters.AddWithValue("@studentcourse", course);
							command.Parameters.AddWithValue("@mregistered", "");
                            command.Parameters.AddWithValue("@mail",encryptionService.Encrypt(email));

						    int rowsAffected = command.ExecuteNonQuery();

                            if(rowsAffected > 0)
                        {
                            Authentication authentication = new Authentication();
                            
                            logging.Logger("ADMIN", "ADD NEW STUDENT", "SUCCESS");
                            string bodytext = $"Greetings,\r\n\r\n You have been successfully added to the Registration System. Below are your login credentials to access the system:\r\n\r\nUsername: {nextusername}\r\nPassword: {gen_pasword}\r\nPlease use these credentials to log in to the system.\r\n\r\nFor security reasons, we recommend changing your password after your first login. If you encounter any issues or have questions regarding your account, please don't hesitate to reach out to our support team.";
                            authentication.Email(bodytext,email,"REGISTERED IN SYSTEM");
                        }
                        else
                        {
                            logging.Logger("ADMIN", "ADD NEW STUDENT", "FAIL");
                        }


					}
					}
				catch (Exception ex)
				{
                    Console.WriteLine(ex.Message);
                    logging.Logger("ADMIN","CONNECTION","TIMEOUT");
				}
			}

		}

	
}
