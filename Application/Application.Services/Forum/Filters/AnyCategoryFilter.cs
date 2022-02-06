using Application.Domain.ApplicationEntities;
using Sonya.AspNetCore.Common.Filtering;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services.Forum.Filters
{
    public class AnyCategoryFilter<T> : IFilter<Thread> where T : ICollection<string>
    {
        public string Description
        {
            get 
            {
                return "Category (Match Any)";
            }
        }

        public T Categories { get; set; }

        public bool IsValid(Thread item)
        {
            if (!Categories.Any())
            {
                return true;
            }

            return item.Categories.Where(x => Categories.Contains(x.NameInUrl)).Any();
        }
    }
}
