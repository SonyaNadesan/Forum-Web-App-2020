using System.Collections.Generic;

namespace Application.Services.Filtering
{
    public interface IFilterService<T>
    {
        List<T> GetFilteredList(List<T> originalList, List<IFilter<T>> filters);

        bool IsValidAgainstAllFilters(T item, List<IFilter<T>> filters);
    }
}