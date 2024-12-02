using Microsoft.VisualBasic;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace RegistrationSystem.Security
{
    public class Monitoring
    {

        public Monitoring() { }


        public void MonitorAdminActivity(string rootPath)
        {
            string logFilePath = Path.Combine(rootPath, "Logging", "logs.txt");
            int loginAttempts = 0;
            string[] logcontentlines = File.ReadAllLines(logFilePath);


            string username = logcontentlines[0].Split(",")[2];

            for (int i = 1; i < logcontentlines.Length; i++)
            {
                string[] logline = logcontentlines[i].Split(",");
                string usr = logline[2];
                string  dateAndTime = logline[0];
                if (loginAttempts >= 6)
                {

                    Authentication authentication = new Authentication();
                    authentication.Email($"A critical security alert that has been detected on our network. Our monitoring systems have identified a possible brute force attack targeting our systems.\r\n\r\nDetails of the Alert:\r\n\r\nDate and Time: {dateAndTime}\r\n user targeted: {usr}\r\nNumber of Failed Login Attempts: {loginAttempts}", "vhuhwavhomokoma@gmail.com", "SECURITY ALERT!");
                    File.WriteAllText(logFilePath, string.Empty);
                    break;
                }

                if (usr == username)
                {
                    if (logline[3] == "ADMIN LOG IN" && logline[4] == "FAIL")
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

        public void MonitorUSERAcvitiy(string rootPath)
        {
            string logFilePath = Path.Combine(rootPath, "Logging", "logs.txt");
            int loginAttempts = 0;
            
            string[] logcontentlines = File.ReadAllLines(logFilePath);
            string username = logcontentlines[0].Split(",")[2];
         
            for (int i = 1; i < logcontentlines.Length; i++)
            {
                string[] logline = logcontentlines[i].Split(",");
                string usr = logline[2];
                string dateAndTime = logline[0];
                if (loginAttempts>=6)
                {
                    
                    Authentication authentication = new Authentication();
                    authentication.Email($"A critical security alert that has been detected on our network. Our monitoring systems have identified a possible brute force attack targeting our systems.\r\n\r\nDetails of the Alert:\r\n\r\nDate and Time: {dateAndTime}\r\n user targeted: {usr}\r\nNumber of Failed Login Attempts: {loginAttempts}", "vhuhwavhomokoma@gmail.com", "SECURITY ALERT!");
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
