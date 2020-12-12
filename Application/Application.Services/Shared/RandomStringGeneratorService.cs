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

            HashAlgorithm algorithm = SHA256.Create();
            var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
