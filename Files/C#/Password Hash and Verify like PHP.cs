using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordHashVerifyLikePHP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string password = "my_secure_password";

            // Hash the password
            string hashedPassword = HashPassword(password);
            Console.WriteLine("Hashed Password: " + hashedPassword);

            // Verify the password
            bool isPasswordValid = VerifyPassword(password, hashedPassword);
            Console.WriteLine("Password valid: " + isPasswordValid);
        }

        private const int SaltSize = 16; // 128 bits
        private const int HashSize = 32; // 256 bits
        private const int Iterations = 10000; // Number of iterations for PBKDF2

        public static string HashPassword(string password)
        {
            // Generate a random salt
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[SaltSize];
                rng.GetBytes(salt);

                // Hash the password with the salt
                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
                {
                    byte[] hash = pbkdf2.GetBytes(HashSize);

                    // Combine salt and hash
                    byte[] hashBytes = new byte[SaltSize + HashSize];
                    Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                    Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

                    // Convert to base64 for storage
                    return Convert.ToBase64String(hashBytes);
                }
            }
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            // Convert the stored hash from base64 to byte array
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            // Extract the salt from the stored hash
            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Extract the hash from the stored hash
            byte[] storedHashValue = new byte[HashSize];
            Array.Copy(hashBytes, SaltSize, storedHashValue, 0, HashSize);

            // Hash the input password with the extracted salt
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);

                // Compare the result
                for (int i = 0; i < HashSize; i++)
                {
                    if (hash[i] != storedHashValue[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
