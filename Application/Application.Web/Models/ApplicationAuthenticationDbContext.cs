using Application.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Application.Web.Models
{
    public class ApplicationAuthenticationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationAuthenticationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
