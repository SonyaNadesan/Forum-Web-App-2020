using System.Collections.Generic;

namespace Application.Web.ViewModels
{
    public interface IPagination
    {
        int CurrentPage { get; set; }

        int StartPage { get; }

        int LastPage { get; }

        int TotalNumberOfResults { get; set; }

        int PageSize { get; set; }

        int MaxNumberOfPagesToShowOnEachRequest { get; set; }

        int NumberOfPages { get; }

        string FormAction { get; set; }

        string FormMethod { get; set; }

        Dictionary<string, string> MoreParametersAndValues { get; set; }
    }
}