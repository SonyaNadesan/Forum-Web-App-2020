using System;
using System.Collections.Generic;

namespace Application.Services.Pagination
{
    public class Pagination<T> : IPagination
    {
        public List<T> ItemsToDisplay { get; internal set; }

        public int PageSize { get; internal set; }

        public int TotalNumberOfResults { get; internal set; }

        public int CurrentPage { get; internal set; }

        public int NumberOfPages { get; internal set; }

        public int StartPage { get; internal set; }

        public int LastPage { get; internal set; }

        public string FormAction { get; internal set; }

        public string FormMethod { get; internal set; }

        public int MaxNumberOfPagesToShowOnEachRequest { get; internal set; }

        public Dictionary<string, string> MoreParametersAndValues { get; internal set; }

        internal Pagination(int currentPage, int pageSize, int startPage, int totalNumberOfResults, int maxNumberOfPagesToShowOnEachRequest)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            MaxNumberOfPagesToShowOnEachRequest = maxNumberOfPagesToShowOnEachRequest;
            TotalNumberOfResults = totalNumberOfResults;
            ItemsToDisplay = new List<T>();
            MoreParametersAndValues = new Dictionary<string, string>();

            SetNumberOfPages();

            SetStartAndLastPages(startPage);
        }

        private void SetNumberOfPages()
        {
            if (TotalNumberOfResults >= PageSize)
            {
                var resultFromDivision = (double)TotalNumberOfResults / PageSize;
                NumberOfPages = (int)Math.Ceiling(resultFromDivision);
            }
            else
            {
                NumberOfPages = 1;
            }
        }

        private void SetStartAndLastPages(int startPage)
        {
            if (MaxNumberOfPagesToShowOnEachRequest == 1)
            {
                StartPage = CurrentPage;
            }
            else
            {
                if (CurrentPage > startPage + (MaxNumberOfPagesToShowOnEachRequest - 1))
                {
                    StartPage = startPage + MaxNumberOfPagesToShowOnEachRequest;
                }
                else if (CurrentPage < startPage)
                {
                    StartPage = startPage - MaxNumberOfPagesToShowOnEachRequest;
                }
                else
                {
                    StartPage = startPage;
                }
            }

            var last = StartPage + (MaxNumberOfPagesToShowOnEachRequest - 1);

            if (last <= NumberOfPages)
            {
                LastPage = last;
            }
            else
            {
                LastPage = NumberOfPages;
            }
        }
    }
}
