using System.Collections.Generic;

namespace Application.Web.ViewModels
{
    public class PaginationViewModel<T>
    {
        public List<T> ItemsToDisplay { get; set; }

        public int PageSize { get; set; }

        public int TotalNumberOfResults { get; set; }

        public int CurrentPage { get; set; }
    }
}
