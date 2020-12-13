using Microsoft.AspNetCore.Identity;

namespace Application.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Salt { get; set; }
        public bool RegistrationConfirmed { get; set; }

        public ApplicationUser() : base()
        {

        }

        public ApplicationUser(string userName) : base(userName)
        {
            UserName = userName;
        }
    }
}
