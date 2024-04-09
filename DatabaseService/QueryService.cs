using RegistrationSystem.Models;
using System.Data.SqlClient;
using RegistrationSystem.Supportfeatures;

namespace RegistrationSystem.DatabaseService
{
    public class QueryService
    {
        public QueryService() { }

        public List<Student> QueryGET()
        {
            string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";

            List<Student> students = new List<Student>();

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
                                students.Add(student);

                            }

                            return students;


                        }



                    }
                }
                catch (Exception)
                {
                    return students;
                }
            }

        }

		public List<Models.Module> QueryModule()
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

		}




		public void queryAddStudent(string fullname,string course)
        {
			string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=60;";


			using (SqlConnection connection = new SqlConnection(connectionString))
			{
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
								string value2 = reader.GetString(1);
								nextusername = support.generateUsername(value2);


							}



						}
					}

						string QueryInsert = @"INSERT INTO Students(ID,StudentName,username,studentpassword,course,modulesRegistered) VALUES (@Value1,@Value2,@Value3,@Value4,@Value5,@Value6)";

						using (SqlCommand command = new SqlCommand(QueryInsert, connection))
						{
							command.Parameters.AddWithValue("@Value1", lastIndex+1);
							command.Parameters.AddWithValue("@Value2", fullname);
							command.Parameters.AddWithValue("@Value3", nextusername);
							command.Parameters.AddWithValue("@Value4", support.RandomPassword());
							command.Parameters.AddWithValue("@Value5", course);
							command.Parameters.AddWithValue("@Value6", "0:");

						    int rowsAffected = command.ExecuteNonQuery();

							if (rowsAffected > 0)
						{
							Console.WriteLine("Entry inserted successfully.");
						}
						else
						{
							Console.WriteLine("No entry inserted.");
						}

					}
					}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
			}

		}

	}
}
