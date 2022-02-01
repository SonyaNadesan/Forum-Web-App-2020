using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Application.Services.Shared
{
    class FlattenHierarchyOfObjectsService<T>
    {
        private PropertyInfo _propertyThatStoresChildren;

        public FlattenHierarchyOfObjectsService(T topLevelItem, PropertyInfo propertyThatStoresChildren)
        {
            var childrenType = propertyThatStoresChildren.PropertyType;

            if(childrenType == typeof(IEnumerable<T>))
            {
                _propertyThatStoresChildren = propertyThatStoresChildren;

                var children = propertyThatStoresChildren.GetValue(topLevelItem) as List<T>;

                var allItemsInOrder = new List<T>();

                foreach (var item in children)
                {
                    allItemsInOrder.Add(item);

                    var childrenOfItem = GetChildren(item);

                    allItemsInOrder.AddRange(childrenOfItem);
                }
            }
        }

        private IEnumerable<T> GetChildren(T item)
        {
            var results = new List<T>();

            DrillDown(item, results);

            return results;
        }

        private void DrillDown(T item, List<T> results)
        {
            var children = (_propertyThatStoresChildren.GetValue(item) as List<T>);

            while (children.Any())
            {
                foreach (var child in children)
                {
                    results.Add(child);

                    DrillDown(child, results);
                }
            }
        }
    }
}