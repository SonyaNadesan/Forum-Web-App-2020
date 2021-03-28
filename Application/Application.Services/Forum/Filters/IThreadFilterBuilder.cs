using Application.Domain;
using Application.Domain.ApplicationEntities;
using Application.Services.Filtering;
using System.Collections.Generic;

namespace Application.Services.Forum.Filters
{
    public interface IThreadFilterBuilder
    {
        IThreadFilterBuilder AddTopicFilter(string topic);

        IThreadFilterBuilder AddCategoryFilter<T>(T categories, Enums.MatchConditions matchCondition) where T : ICollection<string>;

        List<IFilter<Thread>> Build();
    }
}