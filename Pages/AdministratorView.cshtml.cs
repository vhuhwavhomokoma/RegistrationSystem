using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RegistrationSystem.DatabaseService;
using RegistrationSystem.Models;

namespace RegistrationSystem.Pages
{
    public class AdministratorViewModel : PageModel
    {
        public List<Student> studentList = new List<Student>();
        public List<Module> moduleList = new List<Module>();

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

        public IActionResult OnPost()
        {
			QueryService queryService = new QueryService();
			if (studentName == null || studentSurname == null || studentEmail == null || studentCourse == null)
            {
                if (moduleCode == null || moduleName == null || moduleDescription == null || moduleCourse == null)
                {
                    return Page();
                }
				queryService.queryAddModule(moduleCode, moduleName, moduleDescription);
				studentList = queryService.QueryGET();
				moduleList = queryService.QueryModule();

				return Page();
            }
						
			queryService.queryAddStudent(studentName+" "+studentSurname,studentCourse);
			studentList = queryService.QueryGET();
			moduleList = queryService.QueryModule();

			return Page();
        }

        public void OnGet()
        {
            QueryService queryService = new QueryService();
            studentList = queryService.QueryGET();
            moduleList = queryService.QueryModule();
        }
    }
}
