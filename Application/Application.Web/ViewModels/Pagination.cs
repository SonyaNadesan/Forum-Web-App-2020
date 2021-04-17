using System;
using System.Collections.Generic;

namespace Application.Web.ViewModels
{
    public class Pagination<T> : IPagination
    {
        public List<T> ItemsToDisplay { get; set; }

        public int PageSize { get; set; }

        public int TotalNumberOfResults { get; set; }

        public int CurrentPage { get; set; }

        public int NumberOfPages
        {
            get
            {
                var result = 1;

                if (TotalNumberOfResults >= PageSize)
                {
                    var resultFromDivision = (double)TotalNumberOfResults / PageSize;
                    result = (int)Math.Ceiling(resultFromDivision);
                }

                return result;
            }
        }

        public int StartPage
        {
            get
            {
                if (CurrentPage > _startPage + (MaxNumberOfPagesToShowOnEachRequest - 1))
                {
                    return _startPage + MaxNumberOfPagesToShowOnEachRequest;
                }
                else if (CurrentPage < _startPage)
                {
                    return _startPage - MaxNumberOfPagesToShowOnEachRequest;
                }
                else
                {
                    return _startPage;
                }
            }
            set => _startPage = value;
        }

        private int _startPage;

        public int LastPage
        {
            get
            {
                var last = StartPage + (MaxNumberOfPagesToShowOnEachRequest - 1);

                if (last <= NumberOfPages)
                {
                    return last;
                }
                else
                {
                    return NumberOfPages;
                }
            }
        }

        public string FormAction { get; set; }

        public string FormMethod { get; set; }

        public int MaxNumberOfPagesToShowOnEachRequest { get; set; }

        public Dictionary<string, string> MoreParametersAndValues { get; set; }
    }
}
