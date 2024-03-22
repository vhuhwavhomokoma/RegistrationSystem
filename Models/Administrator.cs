namespace RegistrationSystem.Models
{
    public class Administrator: User
    {
        private int _administratorId;
        

        public Administrator(string username, string password, int administratorId): base(username, password) 
            {
        _administratorId = administratorId;
        }

        public int AdministratorId { get {  return _administratorId; } }

    }
}
