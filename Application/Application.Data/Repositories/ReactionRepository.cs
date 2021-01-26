using Application.Domain.ApplicationEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static Application.Domain.Enums;

namespace Application.Data.Repositories
{
    public class ReactionRepository : IReactionRepository
    {
        private readonly ApplicationDbContext Context;

        public ReactionRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public void Add(Reaction reaction)
        {
            if (reaction.ReactionType.ToString() == "NONE")
            {
                Context.Entry(reaction.Thread.User).State = EntityState.Unchanged;
                Context.Entry(reaction.Thread).State = EntityState.Unchanged;
                Context.Entry(reaction.User).State = EntityState.Unchanged;
                Context.Entry(reaction).State = EntityState.Added;
                Context.Reactions.Add(reaction);
            }
        }

        public void Add(User user, Thread thread)
        {
            var reactionFromDb = Context.Reactions.SingleOrDefault(r => r.User.Id == user.Id && r.Thread.Id == thread.Id);

            if (reactionFromDb == null)
            {
                Reaction reaction = new Reaction(Guid.NewGuid(), user, thread, ReactionTypes.LIKE);
                Context.Reactions.Add(reaction);
            }
        }

        public void Delete(Guid reactionId)
        {
            var reaction = Context.Reactions.SingleOrDefault(r => r.ReactionId == reactionId);

            if (reaction != null)
            {
                Context.Reactions.Remove(reaction);
            }
        }

        public Reaction Get(Guid reactionId)
        {
            return (Context.Reactions.Include(r => r.User)).Include(r => r.Thread).Include(r => r.Thread.User).SingleOrDefault(r => r.ReactionId == reactionId);
        }

        public Reaction Get(string email, Guid threadId)
        {
            var reaction = Context.Reactions.Include(r => r.User).Include(r => r.Thread).Include(r => r.Thread.User).SingleOrDefault(r => r.User.Email == email && r.Thread.Id == threadId);
           
            if (reaction == null)
            {
                var user = Context.Users.SingleOrDefault(u => u.Email == email);
                var thread = Context.Threads.SingleOrDefault(t => t.Id == threadId);
                reaction = new Reaction(user, thread, ReactionTypes.NONE);
            }

            return reaction;
        }

        public IEnumerable<Reaction> GetAll()
        {
            var result = Context.Reactions.Include(r => r.User).Include(r => r.Thread).Include(p => p.User);
            return result;
        }

    }
}
