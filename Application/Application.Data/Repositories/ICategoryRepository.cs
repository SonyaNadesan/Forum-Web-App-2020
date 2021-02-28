using Application.Domain.ApplicationEntities;
using System;
using System.Collections.Generic;

namespace Application.Data.Repositories
{
    public interface ICategoryRepository
    {
        Category Get(Guid postId);

        IEnumerable<Category> GetAll();

        void Delete(Guid postId);

        void Add(Category post);

        void Edit(Category post);
    }
}