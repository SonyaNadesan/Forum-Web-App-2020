using System.Collections.Generic;
using System.Linq;

namespace Application.Services.Pagination
{
    public class PaginationHelper
    {
        public static IEnumerable<T> GetItemsToDisplay<T>(IEnumerable<T> allResults, int page, int pageSeize)
        {
            return allResults.Skip((page - 1) * pageSeize).Take(pageSeize).ToList();
        }
    }
}
