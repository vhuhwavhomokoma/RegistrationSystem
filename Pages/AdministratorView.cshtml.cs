using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RegistrationSystem.DatabaseService;
using RegistrationSystem.Models;

namespace RegistrationSystem.Pages
{
    public class AdministratorViewModel : PageModel
    {
        public List<Student> studentList = new List<Student>();


        public void OnGet()
        {
            QueryService queryService = new QueryService();
            studentList = queryService.QueryGET();
        }
    }
}
