using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RegistrationSystem.Pages
{
    public class AdminAuthenticationModel : PageModel
    {
        [BindProperty]
        public string admincode { get; set; } = default!;

        private static string code { get; set; } = default!;


        public IActionResult OnPost()
        {
            if(admincode == null)
            {
                return Page();
            }

            

            if (admincode==code)
            {
                return RedirectToPage("/AdministratorView");
            }

            return Page();
        }

        public void OnGet(string cd)
        {
            code = cd;
            
        }
    }
}
