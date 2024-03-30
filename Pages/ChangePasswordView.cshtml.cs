using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RegistrationSystem.Pages
{
    public class ChangePasswordViewModel : PageModel
    {
        [BindProperty]
        public string enteredUsername { get; set; } = default!;

        public IActionResult OnPost()
        {
            if (enteredUsername==null)
            {
                
                return Page();
            }
        

            return RedirectToPage("/FinalizeChange", new { usr = enteredUsername });
        }

        public void OnGet()
        {
        }
    }
}
