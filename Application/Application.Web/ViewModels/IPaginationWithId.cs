namespace Application.Web.ViewModels
{
    public interface IPaginationWithId : IPagination
    {
        public string Id { get; set; }
    }
}
