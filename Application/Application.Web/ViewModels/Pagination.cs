using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Web.ViewModels
{
    public class Pagination<T> : IPagination
    {
        public List<T> ItemsToDisplay { get; set; }

        public int PageSize { get; set; }

        public int TotalNumberOfResults { get; set; }

        public int CurrentPage { get; set; }

        public string Query { get; set; }

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
                if (CurrentPage > _startPage + 9)
                {
                    return _startPage + 10;
                }
                else if (CurrentPage < _startPage)
                {
                    return _startPage - 10;
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
                var last = StartPage + 9;

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

        public Pagination(IEnumerable<T> allResults, int page, int pageSeize, int startPage, string formAction, string query = "")
        {
            ItemsToDisplay = allResults.Skip((page - 1) * pageSeize).Take(pageSeize).ToList();
            CurrentPage = page;
            PageSize = pageSeize;
            TotalNumberOfResults = allResults.Count();
            Query = query;
            FormAction = formAction;
            StartPage = startPage;
        }
    }
}
