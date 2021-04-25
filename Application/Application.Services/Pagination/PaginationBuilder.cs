﻿namespace Application.Services.Pagination
{
    public class PaginationBuilder<T>
    {
        private Pagination<T> _pagination;

        public PaginationResultsBuilder<T> Create(int currentPage, int pageSize, int startPage, int totalNumberOfResults, int maxNumberOfPagesToShowOnEachRequest)
        {
            _pagination = new Pagination<T>(currentPage, pageSize, startPage, totalNumberOfResults, maxNumberOfPagesToShowOnEachRequest);

            var paginationConfigurationBuilder = new PaginationResultsBuilder<T>(_pagination);

            return paginationConfigurationBuilder;
        }
    }
}