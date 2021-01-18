namespace Application.Web.ViewModels
{
    public interface IPaginationWithId : IPagination
    {
        string Id { get; set; }

        string NameOfIdFieldInView { get; set; }
    }
}
