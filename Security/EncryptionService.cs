using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Security.Cryptography;
using System.Text;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System.Threading.Tasks;


namespace RegistrationSystem.Security
{
    public class EncryptionService
    {
        private readonly SecretClient _secret;
        private const string KeyName = "RegSystemKey1";
        //Encryption and Decryption using AES
        public EncryptionService() {
            var keyVaultUri = "https://regsyskey.vault.azure.net/";
            _secret = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
        }

        private string GetEncryptionKey()
        {
            var secret =  _secret.GetSecret(KeyName);
            return secret.Value.Value;
        }

        public string Encrypt(string plaintext)
        {
            string key = GetEncryptionKey();
            
            Aes aes = Aes.Create();
            
            aes.Key = Encoding.UTF8.GetBytes(key);

            aes.IV = new byte[16];

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key,aes.IV);

            byte[] encryptedtext;


            var msEncrypt = new System.IO.MemoryStream();
                
               using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (var swEncrypt = new System.IO.StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(plaintext);
                    }
                    encryptedtext = msEncrypt.ToArray();
                }
            

            return Convert.ToBase64String(encryptedtext);



        }


        public string Decrypt(string encryptedtext)
        {

            string key = GetEncryptionKey();
            
            byte[] cipherText = Convert.FromBase64String(encryptedtext); ;


            Aes aes = Aes.Create();
           
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = new byte[16];

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            string plaintext;

            var msDecrypt = new System.IO.MemoryStream(cipherText);
                
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new System.IO.StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
            }
                

            return plaintext;
        
        }



    }
}
