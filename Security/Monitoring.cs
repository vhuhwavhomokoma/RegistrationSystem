using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace RegistrationSystem.Security
{
    public class Monitoring
    {

        public Monitoring() { }


        public void MonitorUSERAcvitiy()
        {
            string logFilePath = "Logging/logs.txt";
            int loginAttempts = 0;
            
            string[] logcontentlines = File.ReadAllLines(logFilePath);
            string username = logcontentlines[0].Split(",")[2];
         
            for (int i = 1; i < logcontentlines.Length; i++)
            {
                string[] logline = logcontentlines[i].Split(",");
                string usr = logline[2];
                if (loginAttempts>=6)
                {
                    Console.WriteLine("SECURITY ALERT");
                    File.WriteAllText(logFilePath, string.Empty);
                    break;
                }

                if (usr==username)
                {
                    if (logline[3]== "USER LOG IN" && logline[4]== "FAIL")
                    {
                        loginAttempts += 1;
                    }
                    else
                    {
                        loginAttempts = 0;
                    }
                    
                }
                else
                {
                    username = usr;
                    loginAttempts = 0;
                }
                
            }



        }

    }
}
