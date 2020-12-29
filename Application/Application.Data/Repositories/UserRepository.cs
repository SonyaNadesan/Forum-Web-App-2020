using Application.Domain.ApplicationEntities;
using System.Collections.Generic;
using System.Linq;

namespace Application.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext Context;

        public UserRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public User Get(string userId)
        {
            return Context.Users.SingleOrDefault(u => u.Id == userId);
        }

        public IEnumerable<User> GetAll()
        {
            return Context.Users;
        }

        public void Delete(string userId)
        {
            var user = Context.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
            {
                Context.Users.Remove(user);
            }
        }

        public void Add(User user)
        {
            var userFromDb = Get(user.Id);

            if (userFromDb == null)
            {
                Context.Users.Add(user);
            }
        }

        public void Edit(User user)
        {
            var result = Context.Users.SingleOrDefault(u => u.Id == user.Id);

            if (result != null)
            {
                result.FirstName = user.FirstName;
                result.LastName = user.LastName;
                result.ProfilePictureImageSrc = user.ProfilePictureImageSrc;
            }
        }
    }
}
