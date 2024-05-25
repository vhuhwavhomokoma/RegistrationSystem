using RegistrationSystem.Models;
using System.Data.SqlClient;
using RegistrationSystem.Supportfeatures;
using RegistrationSystem.Security;

namespace RegistrationSystem.DatabaseService
{
    public class QueryService
    {
        public QueryService() { }


        public string QueryAdminEmail()
        {
            try
            {
                string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";
                //initialise encryption service to encrypt or decrypt student data
                EncryptionService encryptionService = new EncryptionService();

                SqlConnection connection = new SqlConnection(connectionString);

                string _email = "";
                connection.Open();


                string queryAdmin = @"SELECT emailaddress FROM Administrators WHERE ID = 1";

                SqlCommand command = new SqlCommand(queryAdmin, connection);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //Decrypt data in order for use in the system
                             _email = reader.GetString(0);


                        }
                    }

                    return _email;

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "";

            }

        }

        public List<Student> QueryGET()
        {
            string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

            List<Student> students = new List<Student>();
            EncryptionService encryptionService = new EncryptionService();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    
                    string queryAllDataSql = @"
                SELECT username, studentpassword, ID, StudentName, course
                FROM Students";

                    using (SqlCommand command = new SqlCommand(queryAllDataSql, connection))
                    {
                        
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
                }
                catch (Exception)
                {
                    Logging logging = new Logging();
                    logging.Logger("USER","CONNECTION","TIMEOUT");

                    return students;
                }
            }

        }

		public void updateModule(string module)
		{
            string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Logging logging = new Logging();
                try
                {
                    connection.Open();

                    // Specify the primary key value or any condition to identify the element to update
                    string primaryKeyValue = module;

                    // SQL command to update the element
                    string updateElementSql = @"
                UPDATE Modules
                SET NumRegistered = NumRegistered + @NewValue1
                WHERE ModuleCode = @PrimaryKeyValue";

                    using (SqlCommand command = new SqlCommand(updateElementSql, connection))
                    {
                        // Set parameter values
                        command.Parameters.AddWithValue("@NewValue1", 1);
                        command.Parameters.AddWithValue("@PrimaryKeyValue", primaryKeyValue);

                        // Execute the command
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            
                            connection.Close();
                            
                        }
                        else
                        {
                            // No matching element found for the update
                            connection.Close();
                            
                        }
                    }
                }
                catch (Exception)
                {
                    logging.Logger("USER","CONNECTION","TIMEOUT");
                }
            }

        }

		public List<Models.Module> QueryModule()
		{
            try
            {
                string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

                List<Models.Module> modules = new List<Models.Module>();

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
                                string value2 = reader.GetString(1);
                                string value3 = reader.GetString(2);
                                string value4 = reader.GetString(3);
                                int value5 = reader.GetInt32(4);
                                Models.Module module = new Models.Module(value1, value2, value3, value4, value5);
                                modules.Add(module);

                            }

                            return modules;

                        }

                    }
                }
            }catch (Exception)
            {
                
                return new List<Models.Module>();
            }

		}

		public void queryAddModule(string modulecode,string modulename,string moduledescription)
		{
			string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";


			using (SqlConnection connection = new SqlConnection(connectionString))
			{
                Logging logging = new Logging();
                try
				{
                    
                    connection.Open();
					
					int lastIndex = 0;
					
					string QueryLastRow = @"SELECT ID FROM Modules WHERE ID=(SELECT max(ID) FROM Modules)";


					using (SqlCommand command = new SqlCommand(QueryLastRow, connection))
					{

						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								lastIndex = reader.GetInt32(0);
								

							}



						}
					}

					string QueryInsert = @"INSERT INTO Modules(ID,ModuleCode,ModuleName,ModuleDetails,NumRegistered) VALUES (@Value1,@Value2,@Value3,@Value4,@Value5)";

					using (SqlCommand command = new SqlCommand(QueryInsert, connection))
					{
						command.Parameters.AddWithValue("@Value1", lastIndex + 1);
						command.Parameters.AddWithValue("@Value2", modulecode);
						command.Parameters.AddWithValue("@Value3", modulename);
						command.Parameters.AddWithValue("@Value4", moduledescription);
						command.Parameters.AddWithValue("@Value5", 0);
						

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



		}

		public void degregisterQuery(int studentid)
		{
            string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                Logging logging = new Logging();
                try
                {
                    
                    connection.Open();

                    string AlterModules = @"SELECT modulesRegistered FROM Students WHERE ID = @Value1";
                    List<string> modulesReg = new List<string>();

                    using (SqlCommand command = new SqlCommand(AlterModules, connection))
                    {
                        command.Parameters.AddWithValue("@Value1", studentid);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                modulesReg = reader.GetString(0).Split(":").ToList();
                               
                            }


                        }

                    }

                    

                    string updateModuleQuery = @"UPDATE Modules SET NumRegistered = NumRegistered - @NewValue1 WHERE ModuleCode = @PrimaryKeyValue";

                    for (int i = 1; i < modulesReg.Count; i++ )
                    {
                        using (SqlCommand command = new SqlCommand(updateModuleQuery, connection))
                        {
                            // Set parameter values
                            command.Parameters.AddWithValue("@NewValue1", 1);
                            command.Parameters.AddWithValue("@PrimaryKeyValue", modulesReg[i]);

                            // Execute the command
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


                    

                    string QueryDeregister = @"DELETE FROM Students WHERE ID = @Value1";

                    using (SqlCommand command = new SqlCommand(QueryDeregister, connection))
                    {
                        command.Parameters.AddWithValue("@Value1", studentid);

                        int rowsAffected = command.ExecuteNonQuery();
                    
                    }

                    

                }
                catch (Exception)
                {
                    logging.Logger("ADMIN", "CONNECTION", "TIMEOUT");
                }
            }


        }


        public void removeQuery(int moduleid)
        {
            string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";


            using (SqlConnection connection = new SqlConnection(connectionString)) { 
                Logging logging = new Logging();

            try
                {
                    

                    connection.Open();



                    string QueryDeregister = @"DELETE FROM Modules WHERE ID = @Value1";

                    using (SqlCommand command = new SqlCommand(QueryDeregister, connection))
                    {
                        command.Parameters.AddWithValue("@Value1", moduleid);

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


        }




        public void queryAddStudent(string fullname,string course,string email)
        {
			string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";
            EncryptionService encryptionService = new EncryptionService();


			using (SqlConnection connection = new SqlConnection(connectionString))
			{
                Logging logging = new Logging();
                try
				{
                    
                    connection.Open();
					Support support = new Support();
					int lastIndex = 0;
					string nextusername = "";
					string QueryLastRow = @"SELECT ID, username FROM Students WHERE ID=(SELECT max(ID) FROM Students)";


					using (SqlCommand command = new SqlCommand(QueryLastRow, connection))
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

						string QueryInsert = @"INSERT INTO Students(ID,StudentName,username,studentpassword,course,modulesRegistered,email_address) VALUES (@Value1,@Value2,@Value3,@Value4,@Value5,@Value6,@Value7)";

						using (SqlCommand command = new SqlCommand(QueryInsert, connection))
						{
                        string gen_pasword = support.RandomPassword();

                            command.Parameters.AddWithValue("@Value1", lastIndex+1);
							command.Parameters.AddWithValue("@Value2", fullname);
							command.Parameters.AddWithValue("@Value3", encryptionService.Encrypt(nextusername));
							command.Parameters.AddWithValue("@Value4", encryptionService.Encrypt(gen_pasword));
							command.Parameters.AddWithValue("@Value5", course);
							command.Parameters.AddWithValue("@Value6", "");
                            command.Parameters.AddWithValue("@Value7",encryptionService.Encrypt(email));

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
}
