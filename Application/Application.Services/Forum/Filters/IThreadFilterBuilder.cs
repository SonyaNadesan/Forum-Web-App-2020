using Application.Domain.ApplicationEntities;
using Application.Services.Filtering;
using System.Collections.Generic;

namespace Application.Services.Forum.Filters
{
    public interface IThreadFilterBuilder
    {
        IThreadFilterBuilder AddTopicFilter(string topic);

        IThreadFilterBuilder AddCategoryFilter<T>(T categories) where T : ICollection<string>;

        List<IFilter<Thread>> Build();
    }
}