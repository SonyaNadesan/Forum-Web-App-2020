using Application.Domain.ApplicationEntities;
using System;
using System.Collections.Generic;

namespace Application.Data.Repositories
{
    public interface ITopicRepository
    {
        Topic Get(Guid topicId);

        IEnumerable<Topic> GetAll();

        void Delete(Guid postId);

        void Add(Topic post);

        void Edit(Topic post);
    }
}