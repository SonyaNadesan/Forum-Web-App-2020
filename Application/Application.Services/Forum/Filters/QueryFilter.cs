using Application.Domain.ApplicationEntities;
using Application.Services.Filtering;

namespace Application.Services.Forum.Filters
{
    public class QueryFilter : IFilter<Thread>
    {
        public string Description { get { return "Query"; } }

        public string Query { get; set; }

        public bool IsValid(Thread item)
        {
            var headingAndBody = (item.Heading + item.Body).ToLower();

            Query = Query.ToLower();

            return Query.Contains(headingAndBody) || headingAndBody.Contains(Query);
        }
    }
}
