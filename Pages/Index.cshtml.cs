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

					// SQL command to check if specific values are present
					string checkValuesSql = @"
                SELECT COUNT(*)
                FROM Students
                WHERE username = @ValueToCheck1 AND studentpassword = @ValueToCheck2";

					using (SqlCommand command = new SqlCommand(checkValuesSql, connection))
					{
						// Set parameter values
						command.Parameters.AddWithValue("@ValueToCheck1", name.ToLower());
						command.Parameters.AddWithValue("@ValueToCheck2", password);

						// Execute the command and read the result
						int rowCount = (int)command.ExecuteScalar();

						if (rowCount > 0)
						{
							// Values are present in the database
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
