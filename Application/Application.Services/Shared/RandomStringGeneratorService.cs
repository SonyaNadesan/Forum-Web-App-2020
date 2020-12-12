using System;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services.Shared
{
    public class RandomStringGeneratorService
    {
        public static string Generate(int length = -1)
        {
            var rand = new Random();
            var randomNumber = rand.Next();

            var password = GetHashString(randomNumber.ToString());

            if (length > 0 && password.Length > length)
            {
                password = password.Substring(0, length);
            }

            return password;
        }

        private static string GetHashString(string inputString)
        {
            var sb = new StringBuilder();

            foreach (byte b in GetHash(inputString))
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        private static byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
    }
}
