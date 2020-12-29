using Application.Domain.ApplicationEntities;
using System;
using System.Collections.Generic;

namespace Application.Data.Repositories
{
    public interface IPostRepository
    {
        Post Get(Guid postId);

        IEnumerable<Post> GetAll();

        void Delete(Guid postId);

        void Add(Post post);

        void Edit(Post post);
    }
}