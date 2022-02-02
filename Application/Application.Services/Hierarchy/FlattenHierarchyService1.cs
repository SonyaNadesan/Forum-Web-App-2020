namespace Application.Services.Hierarchy
{
    class FlattenHierarchyService<T>
    {
        public AncestorConfiguration<T> ConfigureToGetAncestors(T topLevelItem, string nameOfPropertyThatStoresParent)
        {
            return new AncestorConfiguration<T>(topLevelItem, nameOfPropertyThatStoresParent);
        }

        public DescendantConfiguration<T> ConfigureToGetDescendants(T topLevelItem, string nameOfPropertyThatStoresChildren)
        {
            return new DescendantConfiguration<T>(topLevelItem, nameOfPropertyThatStoresChildren);
        }
    }
}