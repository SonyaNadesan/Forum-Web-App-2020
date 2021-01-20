using System.Collections.Generic;
using System.Linq;

namespace Application.Web.ViewModels
{
    public class PaginationWithId<T> : Pagination<T>, IPaginationWithId
    {
        public string Id { get; set; }
        public string NameOfIdFieldInView { get; set; }

        public PaginationWithId(IEnumerable<T> allResults, int page, int pageSeize, int startPage, string formAction, string query = "") : base(allResults, page, pageSeize, startPage, formAction, query = "")
        {
        }
    }
}
