using Application.Domain.ApplicationEntities;
using System.Collections.Generic;

namespace Application.Web.ViewModels
{
    public class ForumIndexViewModel
    {
        public Pagination<Thread> Pagination;
        public string Topic { get; set; }
        public string[] Categories { get; set; }
        public List<Topic> TopicOptions { get; set; }
        public List<Category> CategoryOptions { get; set; }
    }
}
