namespace Application.Domain
{
    public interface IHierarchyItem<TId>
    {
        public TId Id { get; set; }
        public TId ParentId { get; set; }
        public int LevelInHierarchy { get; set; }
        public bool HasParent { get; set; }
    }
}
