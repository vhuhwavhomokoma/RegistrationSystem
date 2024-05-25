using System;
using System.IO;

namespace RegistrationSystem.Security
{
    public class Logging
    {
        public Logging() { }


        public void Logger(string user, string actionevent, string status)
        {
            try
            {
                string logFilePath = "Logging/logs.txt";

                using (StreamWriter writer = File.AppendText(logFilePath))
                {

                    DateTime currentTime = DateTime.Now;

                    writer.WriteLine($"[{currentTime}], User activity: ,{user},{actionevent},{status}");

                }



                    
                
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
