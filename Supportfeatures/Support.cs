using System.Text;

namespace RegistrationSystem.Supportfeatures
{
	public class Support
	{

		public Support() { }

		public string RandomPassword()
		{
			const string possibleCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

			int length = 7;

			StringBuilder password = new StringBuilder();

			Random random = new Random();

		
			for (int i = 0; i < length; i++)
			{
				
				int randomIndex = random.Next(possibleCharacters.Length);

				password.Append(possibleCharacters[randomIndex]);
			}

			
			return password.ToString();
		}

		public string generateUsername(string lastusername)
		{
			int num = int.Parse(lastusername.Substring(1));
			num += 1;
			
			string newusername = num.ToString();
			while (newusername.Length < 7)
			{
				newusername = "0" + newusername;
			}


			return "u"+newusername;
		}





	}
}
