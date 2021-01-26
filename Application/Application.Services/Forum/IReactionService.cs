using Application.Domain.ApplicationEntities;
using System;
using System.Collections.Generic;

namespace Application.Services.Forum
{
    public interface IReactionService
    {
        IEnumerable<Reaction> GetReactionsByThreadId(Guid threadId);

        void Respond(string email, Guid threadId);
    }
}