using System;
using System.Security.Cryptography;
using System.Text;

namespace GreenLifeWinForms.Utils
{
    public static class PasswordHelper
    {
        /// <summary>
        /// Hash a plain-text password using SHA256
        /// </summary>
        public static string HashPassword(string plainPassword)
        {
            if (string.IsNullOrEmpty(plainPassword))
                return string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(plainPassword));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// Verify a plain-text password against a hashed password
        /// </summary>
        public static bool VerifyPassword(string plainPassword, string hashedPassword)
        {
            string hash = HashPassword(plainPassword);
            return string.Equals(hash, hashedPassword, StringComparison.OrdinalIgnoreCase);
        }
    }
}
