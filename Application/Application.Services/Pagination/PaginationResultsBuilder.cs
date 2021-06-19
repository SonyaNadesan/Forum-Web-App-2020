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

    public class PaginationResultsBuilder<TModel, TViewModel>
    {
        private Pagination<TViewModel> _pagination;

        public delegate TViewModel ModelToViewModelConverter(TModel model);

        internal PaginationResultsBuilder(Pagination<TViewModel> pagination)
        {
            _pagination = pagination;
        }

        public PaginationConfigurationBuilder<TViewModel> SeResults(IEnumerable<TModel> results, bool onlyContainsResultsToDisplay, ModelToViewModelConverter converter)
        {
            if (onlyContainsResultsToDisplay)
            {
                foreach (var item in results)
                {
                    var viewModel = converter.Invoke(item);
                    _pagination.ItemsToDisplay.Add(viewModel);
                }

                var paginationConfigurationBuilderForDataAlreadyPaginated = new PaginationConfigurationBuilder<TViewModel>(_pagination);

                return paginationConfigurationBuilderForDataAlreadyPaginated;
            }

            var itemsToDisplay = PaginationHelper.GetItemsToDisplay(results, _pagination.CurrentPage, _pagination.PageSize).ToList();

            foreach (var item in itemsToDisplay)
            {
                var viewModel = converter.Invoke(item);
                _pagination.ItemsToDisplay.Add(viewModel);
            }

            var paginationConfigurationBuilder = new PaginationConfigurationBuilder<TViewModel>(_pagination);

            return paginationConfigurationBuilder;
        }
    }
}