using Application.Domain;
using Application.Domain.ApplicationEntities;
using Sonya.AspNetCore.Common.Filtering;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services.Forum.Filters
{
    public class ThreadFilterBuilder : IThreadFilterBuilder
    {
        private List<IFilter<Thread>> _filters = new List<IFilter<Thread>>();

        private bool _isToBeCleared = false;

        private int _queryFilterPosition = -1;
        private int _categoryFilterPosition = -1;
        private int _topicFilterPosition = -1;

        public IThreadFilterBuilder SetQueryFilter(string query)
        {
            ClearListIfNecessary();

            if (!string.IsNullOrEmpty(query))
            {
                RemoveIfFilterHasBeenAddedPreviously(_queryFilterPosition);

                var queryFilter = new QueryFilter()
                {
                    Query = query
                };

                _filters.Add(queryFilter);

                _queryFilterPosition = _filters.Count() - 1;
            }

            return this;
        }

        public IThreadFilterBuilder SetCategoryFilter<T>(T categories, Enums.MatchConditions matchCondition) where T : ICollection<string>
        {
            ClearListIfNecessary();

            if (matchCondition == Enums.MatchConditions.MatchAny)
            {
                RemoveIfFilterHasBeenAddedPreviously(_categoryFilterPosition);

                var anyCategoryFilter = new AnyCategoryFilter<T>()
                {
                    Categories = categories
                };

                _filters.Add(anyCategoryFilter);
            }
            else if(matchCondition == Enums.MatchConditions.MatchAll)
            {
                RemoveIfFilterHasBeenAddedPreviously(_categoryFilterPosition);

                var allCategoryFilter = new AllCategoryFilter<T>()
                {
                    Categories = categories
                };

                _filters.Add(allCategoryFilter);
            }

            _categoryFilterPosition = _filters.Count() - 1;

            return this;
        }

        public IThreadFilterBuilder SetTopicFilter(string topic)
        {
            ClearListIfNecessary();

            RemoveIfFilterHasBeenAddedPreviously(_topicFilterPosition);

            var topicFilter = new TopicFilter()
            {
                Topic = topic
            };

            _filters.Add(topicFilter);

            _topicFilterPosition = _filters.Count() - 1;

            return this;
        }

        public List<IFilter<Thread>> Build()
        {
            _isToBeCleared = true;

            return _filters;
        }

        private void ClearListIfNecessary()
        {
            if (_isToBeCleared)
            {
                _isToBeCleared = false;
                _filters.Clear();
            }
        }

        private void RemoveIfFilterHasBeenAddedPreviously(int indexOfFilter)
        {
            if(indexOfFilter > -1)
            {
                _filters.RemoveAt(indexOfFilter);
            }
        }
    }
}
