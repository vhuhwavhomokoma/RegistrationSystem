namespace RegistrationSystem.Models
{
    public class Student: User
    {
        private int _studentId;
        private string _studentName;
        private string _course;

        public Student(string username, string password, int studentId, string studentName, string course) : base(username, password)
        {
            _studentId = studentId;
            _studentName = studentName;
            _course = course;

        }

        public int StudentId { get { return _studentId; } }

        public string StudentName { get { return _studentName; } }
        
        public string Course { get { return _course; } }

       

    }
}
