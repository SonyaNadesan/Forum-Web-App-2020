using Sonya.AspNetCore.Common.Pagination;

namespace Application.Web.ViewModels
{
    public class ViewModelWithPagination<TModel, TPagination>
    {
        public TModel PageData { get; set; }
        public Pagination<TPagination> PaginationData { get; set; }
    }
}
