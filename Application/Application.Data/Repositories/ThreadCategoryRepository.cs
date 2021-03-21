using Application.Domain.ApplicationEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Data.Repositories
{
    public class ThreadCategoryRepository : IThreadCategoryRepository
    {
        private readonly ApplicationDbContext Context;

        public ThreadCategoryRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public ThreadCategory Get(Guid threadCategoryId)
        {
            return Context.ThreadCategories.Include(tc => tc.Thread).Include(tc => tc.Category).SingleOrDefault(tc => tc.Id == threadCategoryId);
        }

        public IEnumerable<ThreadCategory> GetAll()
        {
            return Context.ThreadCategories.Include(tc => tc.Thread).Include(tc => tc.Category);
        }

        public void Delete(Guid threadCategoryId)
        {
            var threadCategory = Context.ThreadCategories.SingleOrDefault(tc => tc.Id == threadCategoryId);

            if (threadCategory != null)
            {
                Context.ThreadCategories.Remove(threadCategory);
            }
        }

        public void Add(ThreadCategory threadCategory)
        {
            var threadCategoryFromDb = Get(threadCategory.Id);

            if (threadCategoryFromDb == null)
            {
                Context.ThreadCategories.Add(threadCategory);
            }
        }

        public void Edit(ThreadCategory threadCategory)
        {
            var result = Context.ThreadCategories.SingleOrDefault(tc => tc.Id == threadCategory.Id);

            if (result != null)
            {
                result.Id = threadCategory.Id;
                result.Thread = threadCategory.Thread;
                result.Category = threadCategory.Category;
                result.ThreadId = threadCategory.Thread.Id;
                result.CategoryId = threadCategory.Category.Id;
            }
        }
    }
}
