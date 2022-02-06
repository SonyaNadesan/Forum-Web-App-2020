using Application.Domain.ApplicationEntities;
using Sonya.AspNetCore.Common.Filtering;

namespace Application.Services.Forum.Filters
{
    public class TopicFilter : IFilter<Thread>
    {
        public string Description
        {
            get 
            {
                return "Topic";
            }
        }

        public string Topic { get; set; }

        public bool IsValid(Thread item)
        {
            return Topic == "all" || item.Topic.NameInUrl == Topic;
        }
    }
}
