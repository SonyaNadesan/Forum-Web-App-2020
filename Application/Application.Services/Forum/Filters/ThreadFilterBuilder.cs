using Application.Domain;
using Application.Domain.ApplicationEntities;
using Sonya.AspNetCore.Common.Filtering;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services.Forum.Filters
{
    public class ThreadFilterBuilder : FilterBuilder<Thread>, IThreadFilterBuilder
    {
        private int _queryFilterPosition = -1;
        private int _categoryFilterPosition = -1;
        private int _topicFilterPosition = -1;

        public IThreadFilterBuilder SetQueryFilter(string query)
        {
            if (!string.IsNullOrEmpty(query))
            {
                var queryFilter = new QueryFilter()
                {
                    Query = query
                };

                SetFilter(queryFilter, ref _queryFilterPosition);
            }

            return this;
        }

        public IThreadFilterBuilder SetCategoryFilter<T>(T categories, Enums.MatchConditions matchCondition) where T : ICollection<string>
        {
            if (matchCondition == Enums.MatchConditions.MatchAny)
            {
                var anyCategoryFilter = new AnyCategoryFilter<T>()
                {
                    Categories = categories
                };

                SetFilter(anyCategoryFilter, ref _categoryFilterPosition);
            }
            else if(matchCondition == Enums.MatchConditions.MatchAll)
            {
                var allCategoryFilter = new AllCategoryFilter<T>()
                {
                    Categories = categories
                };

                SetFilter(allCategoryFilter, ref _categoryFilterPosition);
            }

            return this;
        }

        public IThreadFilterBuilder SetTopicFilter(string topic)
        {
            var topicFilter = new TopicFilter()
            {
                Topic = topic
            };

            SetFilter(topicFilter, ref _topicFilterPosition);

            return this;
        }
    }
}
