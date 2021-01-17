using Application.Domain.ApplicationEntities;
using System.Collections.Generic;

namespace Application.Data.Repositories
{
    public interface IUserRepository
    {
        User Get(string email);

        IEnumerable<User> GetAll();

        void Delete(string userId);
        void Add(User user);

        void Edit(User user);
    }
}