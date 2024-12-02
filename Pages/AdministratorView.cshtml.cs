using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RegistrationSystem.DatabaseService;
using RegistrationSystem.Models;
using RegistrationSystem.Security;

namespace RegistrationSystem.Pages
{
    public class AdministratorViewModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public List<Student> studentList = new List<Student>();
        public List<Module> moduleList = new List<Module>();
        public List<SelectListItem> Options = new List<SelectListItem>
            {
                new SelectListItem { Value = "Computer Science", Text = "Computer Science" },
                new SelectListItem { Value = "Accounting", Text = "Accounting" },
                new SelectListItem { Value = "Law", Text = "Law" },
                new SelectListItem { Value = "Informatics", Text = "Informatics" }
            };

        [BindProperty]
        public string studentName { get; set; } = default!;

        [BindProperty]
        public string studentSurname { get; set; } = default!;

        [BindProperty]
        public string studentEmail { get; set; } = default!;

        [BindProperty]
        public string studentCourse { get; set; } = default!;

        [BindProperty]
        public string moduleCode { get; set; } = default!;

        [BindProperty]
        public string moduleName { get; set; } = default!;

        [BindProperty]
        public string moduleDescription { get; set; } = default!;

        [BindProperty]
        public string moduleCourse { get; set; } = default!;

        [BindProperty]
        public string deregister {  get; set; } = default!;

        [BindProperty]
        public string remove { get; set; } = default!;


        public AdministratorViewModel(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public  IActionResult OnPost()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            QueryService queryService = new QueryService(webRootPath);
            EncryptionService encryptionService = new EncryptionService();
            
            if (remove == null) {
                if (deregister == null)
                {

                    if (studentName == null || studentSurname == null || studentEmail == null || studentCourse == null)
                    {
                        if (moduleCode == null || moduleName == null || moduleDescription == null || moduleCourse == null)
                        {
                          
                            return Page();
                        }
                        queryService.queryAddModule(moduleCode, moduleName, moduleDescription,moduleCourse);
                        
                        studentList = queryService.QueryGET();
                        moduleList = queryService.QueryModule();

                        return Page();
                    }

                    queryService.queryAddStudent(encryptionService.Encrypt(studentName + " " + studentSurname), encryptionService.Encrypt(studentCourse),studentEmail);
                    
                    studentList = queryService.QueryGET();
                    moduleList = queryService.QueryModule();

                    return Page();



                }
                queryService.degregisterQuery(int.Parse(deregister));
                studentList = queryService.QueryGET();
                moduleList = queryService.QueryModule();

                return Page();

            }
            queryService.removeQuery(int.Parse(remove));
            studentList = queryService.QueryGET();
            moduleList = queryService.QueryModule();

            return Page();
        }

        public void OnGet()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;
            QueryService queryService = new QueryService(webRootPath);
            studentList = queryService.QueryGET();
            moduleList = queryService.QueryModule();
        }
    }
}
