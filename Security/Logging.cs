using System;
using System.IO;

namespace RegistrationSystem.Security
{
    public class Logging
    {
        string filePath;
        public Logging(string rootPath) {
        filePath = Path.Combine(rootPath, "Logging", "logs.txt");
        }


        public void Logger(string user, string actionevent, string status)
        {
            try
            {
                

                using (StreamWriter writer = File.AppendText(filePath))
                {

                    DateTime currentTime = DateTime.Now;

                    writer.WriteLine($"[{currentTime}], User activity: ,{user},{actionevent},{status}");

                }



                    
                
            }catch (Exception)
            {
                
            }

        }
    }
}
