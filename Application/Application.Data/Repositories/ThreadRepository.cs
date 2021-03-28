using Application.Domain.ApplicationEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Data.Repositories
{
    public class ThreadRepository : IThreadRepository
    {
        private readonly ApplicationDbContext Context;

        public ThreadRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public Thread Get(Guid threadId)
        {
            return Context.Threads.Include(t => t.User)
                                  .Include(t => t.Posts)
                                  .Include(t => t.Reactions)
                                  .Include(t => t.Categories)
                                  .Include(t => t.Topic)
                                  .SingleOrDefault(t => t.Id == threadId);
        }

        public IEnumerable<Thread> GetAll()
        {
            return Context.Threads.Include(t => t.User)
                                  .Include(t => t.Posts)
                                  .Include(t => t.Reactions)
                                  .Include(t => t.Categories)
                                  .Include(t => t.Topic);
        }

        public void Delete(Guid threadId)
        {
            var thread = Get(threadId);

            if (thread != null)
            {
                Context.Threads.Remove(thread);
            }
        }

        public void Add(Thread thread)
        {
            var threadFromDb = Get(thread.Id);

            if (threadFromDb == null)
            {
                Context.Entry(thread.User).State = EntityState.Unchanged;
                Context.Entry(thread.Topic).State = EntityState.Unchanged;
                Context.Threads.Add(thread);
            }
        }

        public void Edit(Thread thread)
        {
            var result = Context.Threads.Include(t => t.User)
                                        .Include(t => t.Posts)
                                        .Include(t => t.Reactions)
                                        .Include(t => t.Categories)
                                        .Include(t => t.Topic).SingleOrDefault(t => t.Id == thread.Id);

            if (result != null)
            {
                result.User = thread.User;
                result.Heading = thread.Heading;
                result.Body = thread.Body;
                result.DateTime = thread.DateTime;
            }
        }
    }
}
