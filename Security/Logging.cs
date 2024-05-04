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
                    // Get current timestamp
                    DateTime currentTime = DateTime.Now;

                    // Log user activity
                    writer.WriteLine($"[{currentTime}] User activity: {user} {actionevent} {status}");

                    // Inform user that activity has been logged
                    Console.WriteLine("Activity logged successfully.");
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
