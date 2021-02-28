using System;

namespace Application.Domain.ApplicationEntities
{
    public class Topic
    {
        public Guid Id { get; set; }
        public string NameInUrl { get; set; }
        public string DisplayName { get; set; }
    }
}
