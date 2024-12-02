namespace RegistrationSystem.Models
{
    public class Module
    {
        private int _id;
        private string _moduleCode;
        private string _moduleName;
        private string _moduleDetails;
        private int _numRegistered;
        private string _modulecourse;


        public Module(int id, string moduleCode, string moduleName, string moduleDetails, int numRegistered, string modulecourse)
        {
            _id = id;
            _moduleCode = moduleCode;
            _moduleName = moduleName;
            _moduleDetails = moduleDetails;
            _numRegistered = numRegistered;
            _modulecourse = modulecourse;
        }

        public int Id { get { return _id; } }

        public string ModuleCode { get { return _moduleCode; } }

        public string ModuleName { get { return _moduleName; } }

        public string ModuleDetails { get { return _moduleDetails; } }

        public int NumRegistered { get { return _numRegistered; } }

        public string ModuleCourse { get { return _modulecourse; } }


    }
}
