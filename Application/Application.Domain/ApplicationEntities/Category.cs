using System;

namespace Application.Domain.ApplicationEntities
{
    public class Category
    {
        public Guid Id { get; set; }
        public string NameInUrl { get; set; }
        public string DisplayName { get; set; }
    }
}
