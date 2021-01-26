using Application.Domain.ApplicationEntities;
using System;
using System.Collections.Generic;

namespace Application.Data.Repositories
{
    public interface IReactionRepository
    {
        void Add(Reaction reaction);

        void Add(User user, Thread thread);

        void Delete(Guid reactionId);

        Reaction Get(Guid reactionId);

        Reaction Get(string email, Guid thread);

        IEnumerable<Reaction> GetAll();
    }
}