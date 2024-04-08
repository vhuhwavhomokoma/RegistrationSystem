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


        public IActionResult OnPost()
        {
            if (studentName == null || studentSurname == null || studentEmail == null || studentCourse == null)
            {
                return Page();
            }
			QueryService queryService = new QueryService();
            queryService.queryAddStudent(studentName+" "+studentSurname,studentCourse);

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
