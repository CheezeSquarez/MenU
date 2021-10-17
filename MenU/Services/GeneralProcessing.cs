using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace MenU.Services
{
    class GeneralProcessing
    {
        //This method will be used to calculate a hash from a desired string
        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
            
        }


        /// <summary>
        /// Recieves the raw password, the salt and the number of hash iterations
        /// and runs the password with the salt through the hash function, the desired amount of times (iterations)
        /// </summary>
        /// <param name="pass">string containing the raw password</param>
        /// <param name="salt">string containing the salt of the logged in user</param>
        /// <param name="iterations">int specifying how many times the password and salt should be hashed</param>
        /// <returns>Hashed password calculated with string and hashed the desired ammount of times</returns>
        public static string PasswordToHash(string pass, string salt, int iterations)
        {
            string hash = pass + salt;

            for (int i = 0; i < iterations; i++)
            {
                hash = ComputeSha256Hash(hash);
            }
            return hash;
        }
    }
}
