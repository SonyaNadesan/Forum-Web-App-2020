using Application.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services.Shared
{
    class FlattenHierarchyService<T, TId> where T : IHierarchyItem<TId>
    {
        private readonly List<T> _allItemsUnordered;

        public FlattenHierarchyService(List<T> allItems)
        {
            _allItemsUnordered = allItems;
        }

        public List<T> Flatten(SharedDelegates.IsEqual<TId> isEqualMethod)
        {
            var topLevelItems = _allItemsUnordered.Where(x => x.LevelInHierarchy == 1);

            var flattenedHierarchy = new List<T>();

            foreach (var item in topLevelItems)
            {
                flattenedHierarchy.Add(item);

                var children = GetChildren(item, isEqualMethod);

                flattenedHierarchy.AddRange(children);
            }

            return flattenedHierarchy;
        }

        private List<T> GetChildren(T item, SharedDelegates.IsEqual<TId> isEqualMethod)
        {
            var children = new List<T>();

            DrillDown(_allItemsUnordered, item, children, isEqualMethod);

            return children;
        }

        private void DrillDown(List<T> allItemsUnordered, T item, List<T> results, SharedDelegates.IsEqual<TId> isEqualMethod)
        {
            var children = _allItemsUnordered.Where(x => isEqualMethod.Invoke(x.Id, item.Id) && x.HasParent && isEqualMethod.Invoke(x.ParentId, item.Id)).ToList();

            var drillDown = children.Any();

            while (drillDown)
            {
                foreach (var child in children)
                {
                    results.Add(child);

                    DrillDown(allItemsUnordered, child, results, isEqualMethod);
                }

                drillDown = false;
            }
        }
    }
}
