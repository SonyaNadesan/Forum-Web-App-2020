using Application.Domain.ApplicationEntities;
using Application.Services.Filtering;
using System.Collections.Generic;

namespace Application.Services.Forum.Filters
{
    public class ThreadFilterBuilder : IThreadFilterBuilder
    {
        private List<IFilter<Thread>> _filters = new List<IFilter<Thread>>();

        private bool isToBeCleared = false;

        public IThreadFilterBuilder AddCategoryFilter<T>(T categories) where T : ICollection<string>
        {
            ClearListIfNecessary();

            var categoryFilter = new CategoryFilter<T>()
            {
                Categories = categories
            };

            _filters.Add(categoryFilter);

            return this;
        }

        public IThreadFilterBuilder AddTopicFilter(string topic)
        {
            ClearListIfNecessary();

            var topicFilter = new TopicFilter()
            {
                Topic = topic
            };

            _filters.Add(topicFilter);

            return this;
        }

        public List<IFilter<Thread>> Build()
        {
            isToBeCleared = true;
            return _filters;
        }

        private void ClearListIfNecessary()
        {
            if (isToBeCleared)
            {
                isToBeCleared = false;
                _filters.Clear();
            }
        }
    }
}
