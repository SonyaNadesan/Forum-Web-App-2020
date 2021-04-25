using System.Collections.Generic;
using System.Linq;

namespace Application.Services.Pagination
{
    public class PaginationResultsBuilder<T>
    {
        private Pagination<T> _pagination;

        internal PaginationResultsBuilder(Pagination<T> pagination)
        {
            _pagination = pagination;
        }

        public PaginationConfigurationBuilder<T> SeResults(IEnumerable<T> results, bool onlyContainsResultsToDisplay)
        {
            if (onlyContainsResultsToDisplay)
            {
                _pagination.ItemsToDisplay = results.ToList();

                var paginationConfigurationBuilderForDataAlreadyPaginated = new PaginationConfigurationBuilder<T>(_pagination);

                return paginationConfigurationBuilderForDataAlreadyPaginated;
            }

            _pagination.ItemsToDisplay = PaginationHelper.GetItemsToDisplay<T>(results, _pagination.CurrentPage, _pagination.PageSize).ToList();

            var paginationConfigurationBuilder = new PaginationConfigurationBuilder<T>(_pagination);

            return paginationConfigurationBuilder;
        }
    }
}
