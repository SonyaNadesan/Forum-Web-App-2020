using Application.Domain.ApplicationEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext Context;

        public CategoryRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public Category Get(Guid categoryId)
        {
            return Context.Categories.Include(c => c.Threads).SingleOrDefault(c => c.Id == categoryId);
        }

        public IEnumerable<Category> GetAll()
        {
            return Context.Categories;
        }

        public void Delete(Guid categoryId)
        {
            var category = Context.Categories.SingleOrDefault(c => c.Id == categoryId);

            if (category != null)
            {
                Context.Categories.Remove(category);
            }
        }

        public void Add(Category category)
        {
            var categoryFromDb = Get(category.Id);

            if (categoryFromDb == null)
            {
                Context.Categories.Add(category);
            }
        }

        public void Edit(Category category)
        {
            var result = Context.Categories.SingleOrDefault(c => c.Id == category.Id);

            if (result != null)
            {
                result.Id = category.Id;
                result.NameInUrl = category.NameInUrl;
                result.DisplayName = category.DisplayName;
            }
        }
    }
}
