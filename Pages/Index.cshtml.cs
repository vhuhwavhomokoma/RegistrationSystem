using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace RegistrationSystem.Pages
{
    public class IndexModel : PageModel
    {

		[BindProperty]
		public string nm { get; set; } = default!;
		[BindProperty]
		public string pw { get; set; } = default!;


		private bool Query(string name, string password)
		{
			string connectionString = "Server=tcp:myserver098.database.windows.net,1433;Initial Catalog=LibraryDB;Persist Security Info=False;User ID=veemokoma;Password=libraryweb4$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=90;";

			using (SqlConnection connection = new SqlConnection(connectionString))
			{
				try
				{
					connection.Open();

					// check if login values are valid and present in database
					string checkValuesSql = @"
                SELECT COUNT(*)
                FROM Students
                WHERE username = @ValueToCheck1 AND studentpassword = @ValueToCheck2";

					using (SqlCommand command = new SqlCommand(checkValuesSql, connection))
					{
						
						command.Parameters.AddWithValue("@ValueToCheck1", name.ToLower());
						command.Parameters.AddWithValue("@ValueToCheck2", password);

					
						int rowCount = (int)command.ExecuteScalar();

						if (rowCount > 0)
						{
							
							return true;
						}
						else
						{
							return false;
						}
					}
				}
				catch (Exception ex)
				{
					return false;
				}
			}
		}



		public IActionResult OnPost()
		{
			if (pw != null && nm != null)
			{
				if (Query(nm, pw))
				{
					return RedirectToPage("/StudentView");
				}
			}

			return Page();
		}

		public void OnGet()
        {

        }
    }
}
