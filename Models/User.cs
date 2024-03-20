namespace RegistrationSystem.Models
{
    public class User
    {
        private string _username;
        private string _password;
        public User(string username, string password) { 
        
        _username = username;
        _password = password;
        
        }

        public string UserName { get { return _username; } }
        public string Password { get { return _password; } }


    }
}
