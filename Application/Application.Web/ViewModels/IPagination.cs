namespace Application.Web.ViewModels
{
    public interface IPagination
    {
        int CurrentPage { get; set; }

        int StartPage { get; }

        int LastPage { get; }

        int TotalNumberOfResults { get; set; }

        int PageSize { get; set; }

        int NumberOfPages { get; }

        string Query { get; set; }

        string FormAction { get; set; }
    }
}