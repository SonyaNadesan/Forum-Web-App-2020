using Application.Domain.ApplicationEntities;
using System;
using System.Collections.Generic;

namespace Application.Data.Repositories
{
    public interface IThreadCategoryRepository
    {
        ThreadCategory Get(Guid threadCategoryId);
        IEnumerable<ThreadCategory> GetAll();
        void Delete(Guid threadCategoryId);
        void Add(ThreadCategory threadCategory);
        void Edit(ThreadCategory threadCategory);
    }
}