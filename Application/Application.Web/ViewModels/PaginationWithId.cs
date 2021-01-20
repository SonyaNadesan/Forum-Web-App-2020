using System.Collections.Generic;

namespace Application.Web.ViewModels
{
    public class PaginationWithId<T> : Pagination<T>, IPaginationWithId
    {
        public string Id { get; set; }
        public string NameOfIdFieldInView { get; set; }
    }
}
