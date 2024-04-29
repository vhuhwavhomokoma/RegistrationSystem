using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Security.Cryptography;
using System.Text;


namespace RegistrationSystem.Security
{
    public class EncryptionService
    {
        //Encryption and Decryption using AES
        public EncryptionService() { }

        public string Encrypt(string plaintext)
        {
            string key = "1234567891234567";

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

            string key = "1234567891234567";

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
