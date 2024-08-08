using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //string password = "user_password";
            //string hashedPassword = PasswordHasher.HashPassword(password);

            //// Store `hashedPassword` in your database.

            //// When verifying a login attempt:
            //string enteredPassword = "user_password"; // This should be the password entered by the user
            //bool isPasswordCorrect = PasswordHasher.VerifyPassword(enteredPassword, hashedPassword);

            //Console.WriteLine($"Password correct: {isPasswordCorrect}");

            string domain = "domain"; // Use "." for local machine
            string username = "user_name";
            string password = "password";

            bool isValid = WindowsAuth.ValidateUser(domain, username, password);
            Console.WriteLine($"Credentials are valid: {isValid}");
        }
    }

    public static class PasswordHasher
    {
        private const int SaltSize = 16; // 128 bit
        private const int HashSize = 20; // 160 bit
        private const int Iterations = 10000; // Number of iterations for PBKDF2

        public static string HashPassword(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[SaltSize];
                rng.GetBytes(salt);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
                {
                    byte[] hash = pbkdf2.GetBytes(HashSize);

                    byte[] hashBytes = new byte[SaltSize + HashSize];
                    Array.Copy(salt, 0, hashBytes, 0, SaltSize);
                    Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

                    return Convert.ToBase64String(hashBytes);
                }
            }
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            byte[] hash = new byte[HashSize];
            Array.Copy(hashBytes, SaltSize, hash, 0, HashSize);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] testHash = pbkdf2.GetBytes(HashSize);

                for (int i = 0; i < HashSize; i++)
                {
                    if (hash[i] != testHash[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    public class WindowsAuth
    {
        private const int LOGON32_PROVIDER_DEFAULT = 0;
        private const int LOGON32_LOGON_INTERACTIVE = 2;
        private const int LOGON32_LOGON_BATCH = 4;

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool LogonUser(
            string lpszUsername,
            string lpszDomain,
            string lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            out IntPtr phToken);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hObject);

        public static bool ValidateUser(string domain, string username, string password)
        {
            IntPtr tokenHandle = IntPtr.Zero;

            try
            {
                bool result = LogonUser(username, domain, password, LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT, out tokenHandle);

                if (result)
                {
                    // The credentials are valid
                    return true;
                }
                else
                {
                    // The credentials are invalid
                    return false;
                }
            }
            finally
            {
                if (tokenHandle != IntPtr.Zero)
                {
                    CloseHandle(tokenHandle);
                }
            }
        }
    }
}
