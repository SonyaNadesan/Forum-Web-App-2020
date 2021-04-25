using System.Collections.Generic;

namespace Application.Services.Pagination
{
    public interface IPagination
    {
        int CurrentPage { get;}

        int StartPage { get; }

        int LastPage { get; }

        int TotalNumberOfResults { get; }

        int PageSize { get; }

        int MaxNumberOfPagesToShowOnEachRequest { get; }

        int NumberOfPages { get; }

        string FormAction { get; }

        string FormMethod { get; }

        Dictionary<string, string> MoreParametersAndValues { get; }
    }
}
