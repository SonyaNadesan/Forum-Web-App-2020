using System.Collections.Generic;

namespace Application.Services.Filtering
{
    public class FilterService<T> : IFilterService<T>
    {
        public List<T> GetFilteredList(List<T> originalList, List<IFilter<T>> filters)
        {
            for (int i = 0; i < originalList.Count; i++)
            {
                foreach (var filter in filters)
                {
                    if (!filter.IsValid(originalList[i]))
                    {
                        originalList.RemoveAt(i);
                        i = i - 1;
                        break;
                    }
                }
            }

            return originalList;
        }

        public bool IsValidAgainstAllFilters(T item, List<IFilter<T>> filters)
        {
            foreach (var filter in filters)
            {
                if (!filter.IsValid(item))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
