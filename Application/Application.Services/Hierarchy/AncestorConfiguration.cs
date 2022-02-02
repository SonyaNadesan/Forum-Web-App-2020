using System;
using System.Collections.Generic;
using System.Reflection;

namespace Application.Services.Hierarchy
{
    internal class AncestorConfiguration<T>
    {
        private PropertyInfo _propertyThatStoresParent;
        private Type _parentType;
        private T _item;

        public AncestorConfiguration(T item, string nameOfPropertyThatStoresParent)
        {
            _item = item;
            _propertyThatStoresParent = typeof(T).GetProperty(nameOfPropertyThatStoresParent);
            _parentType = _propertyThatStoresParent.PropertyType;
        }

        public List<T> Flatten()
        {
            var allItemsInOrder = new List<T>();

            if (_parentType == typeof(T))
            {
                var parent = (T) _propertyThatStoresParent.GetValue(_item);

                while(parent != null)
                {
                    allItemsInOrder.Add(parent);

                    parent = (T)_propertyThatStoresParent.GetValue(parent);
                }
            }

            return allItemsInOrder;
        }
    }
}