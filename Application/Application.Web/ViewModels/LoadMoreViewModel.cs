using System.Collections.Generic;

namespace Application.Web.ViewModels
{
    public class LoadMoreViewModel<T>
    {
        public List<T> ItemsToDisplay { get; set; }

        public string Id { get; set; }

        public int From { get; set; }

        public int Take { get; set; }

        public bool HasMore { get; set; }
    }
}
