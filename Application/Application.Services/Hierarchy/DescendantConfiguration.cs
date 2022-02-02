using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Application.Services.Hierarchy
{
    internal class DescendantConfiguration<T>
    {
        private PropertyInfo _propertyThatStoresChildren;
        private T _item;

        public DescendantConfiguration(T item, string nameOfPropertyThatStoresChildren)
        {
            _item = item;
            _propertyThatStoresChildren = typeof(T).GetProperty(nameOfPropertyThatStoresChildren); ;
        }

        public List<T> Flatten()
        {
            var allItemsInOrder = new List<T>();

            var children = _propertyThatStoresChildren.GetValue(_item);

            if (children is IEnumerable<T>)
            {
                var childList = (List<T>) _propertyThatStoresChildren.GetValue(_item);

                foreach (var item in childList)
                {
                    allItemsInOrder.Add(item);

                    GetDescendants(item, allItemsInOrder);
                }
            }

            return allItemsInOrder;
        }

        private void GetDescendants(T item, List<T> results)
        {
            var children = (List<T>) _propertyThatStoresChildren.GetValue(item);

            var anyChildren = children.Any();

            while (anyChildren)
            {
                foreach (var child in children)
                {
                    results.Add(child);

                    GetDescendants(child, results);
                }

                anyChildren = false;
            }
        }
    }
}