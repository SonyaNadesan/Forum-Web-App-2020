using Application.Domain.ApplicationEntities;
using System;
using System.Collections.Generic;

namespace Application.Data.Repositories
{
    public interface IThreadRepository
    {
        Thread Get(Guid threadId);

        IEnumerable<Thread> GetAll();

        void Delete(Guid threadId);

        void Add(Thread thread);

        void Edit(Thread thread);
    }
}