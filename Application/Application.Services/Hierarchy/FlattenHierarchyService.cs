using Application.Domain;
using Application.Services.Shared;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services.Hierarchy
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

                var children = GetDescendants(item, isEqualMethod);

                flattenedHierarchy.AddRange(children);
            }

            return flattenedHierarchy;
        }

        public List<T> GetDescendants(T item, SharedDelegates.IsEqual<TId> isEqualMethod)
        {
            var children = new List<T>();

            DrillDown(_allItemsUnordered, item, children, isEqualMethod);

            return children;
        }

        public List<T> GetAncestors(T item, SharedDelegates.IsEqual<TId> isEqualMethod)
        {
            var ancestors = new List<T>();

            while(item != null && item.HasParent)
            {
                ancestors.Add(item);

                item = _allItemsUnordered.SingleOrDefault(x => isEqualMethod.Invoke(x.Id, item.ParentId));
            }

            return ancestors;
        }

        private void DrillDown(List<T> allItemsUnordered, T item, List<T> results, SharedDelegates.IsEqual<TId> isEqualMethod)
        {
            var children = _allItemsUnordered.Where(x => x.HasParent && isEqualMethod.Invoke(x.ParentId, item.Id)).ToList();

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
