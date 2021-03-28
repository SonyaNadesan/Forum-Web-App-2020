using Application.Domain;
using Application.Domain.ApplicationEntities;
using Application.Services.Filtering;
using System.Collections.Generic;

namespace Application.Services.Forum.Filters
{
    public class ThreadFilterBuilder : IThreadFilterBuilder
    {
        private List<IFilter<Thread>> _filters = new List<IFilter<Thread>>();

        private bool isToBeCleared = false;

        public IThreadFilterBuilder AddQueryFilter(string query)
        {
            ClearListIfNecessary();

            if (!string.IsNullOrEmpty(query))
            {
                var queryFilter = new QueryFilter()
                {
                    Query = query
                };

                _filters.Add(queryFilter);
            }

            return this;
        }

        public IThreadFilterBuilder AddCategoryFilter<T>(T categories, Enums.MatchConditions matchCondition) where T : ICollection<string>
        {
            ClearListIfNecessary();

            if (matchCondition == Enums.MatchConditions.MatchAny)
            {
                var anyCategoryFilter = new AnyCategoryFilter<T>()
                {
                    Categories = categories
                };

                _filters.Add(anyCategoryFilter);
            }
            else if(matchCondition == Enums.MatchConditions.MatchAll)
            {
                var allCategoryFilter = new AllCategoryFilter<T>()
                {
                    Categories = categories
                };

                _filters.Add(allCategoryFilter);
            }

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
