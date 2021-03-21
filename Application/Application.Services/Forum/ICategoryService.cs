using Application.Domain.ApplicationEntities;
using System;
using System.Collections.Generic;

namespace Application.Services.Forum
{
    public interface ICategoryService
    {
        ServiceResponse<IEnumerable<Category>> GetAll();
        ServiceResponse<Category> Get(Guid categoryId);
        ServiceResponse<Category> Edit(Category category);
        ServiceResponse<Category> Delete(Category category);
        ServiceResponse<Category> Create(string nameInUrl, string displayName);
    }
}