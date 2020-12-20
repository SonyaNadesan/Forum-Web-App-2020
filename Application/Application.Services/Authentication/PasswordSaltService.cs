namespace Application.Services.Authentication
{
    public class PasswordSaltService
    {
        public static string GetPasswordWithSalt(string password, string salt)
        {
            return password + salt;
        }
    }
}
