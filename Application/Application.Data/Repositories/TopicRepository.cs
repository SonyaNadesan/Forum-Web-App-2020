using Application.Domain.ApplicationEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Data.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        private readonly ApplicationDbContext Context;

        public TopicRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public Topic Get(Guid topicId)
        {
            return Context.Topics.SingleOrDefault(t => t.Id == topicId);
        }

        public IEnumerable<Topic> GetAll()
        {
            return Context.Topics;
        }

        public void Delete(Guid topicId)
        {
            var thread = Context.Threads.SingleOrDefault(t => t.Id == topicId);

            if (thread != null)
            {
                Context.Threads.Remove(thread);
            }
        }

        public void Add(Topic topic)
        {
            var topicFromDb = Get(topic.Id);

            if (topicFromDb == null)
            {
                Context.Topics.Add(topic);
            }
        }

        public void Edit(Topic topic)
        {
            var result = Context.Topics.SingleOrDefault(t => t.Id == topic.Id);

            if (result != null)
            {
                result.Id = topic.Id;
                result.NameInUrl = topic.NameInUrl;
                result.DisplayName = topic.DisplayName;
            }
        }
    }
}
