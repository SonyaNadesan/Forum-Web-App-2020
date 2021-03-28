using Application.Domain.ApplicationEntities;
using Application.Services.Filtering;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services.Forum.Filters
{
    public class CategoryFilter<T> : IFilter<Thread> where T : ICollection<string>
    {
        public string Description
        {
            get 
            {
                return "Category";
            }
        }

        public T Categories { get; set; }

        public bool IsValid(Thread item)
        {
            if (!Categories.Any())
            {
                return true;
            }

            if (Categories.Any())
            {
                return item.Categories.Where(x => Categories.Contains(x.NameInUrl)).Any();
            }

            return false;
        }
    }
}
