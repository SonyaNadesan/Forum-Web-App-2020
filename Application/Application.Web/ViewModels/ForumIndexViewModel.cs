using Application.Domain.ApplicationEntities;
using Application.Services.Pagination;
using System.Collections.Generic;

namespace Application.Web.ViewModels
{
    public class ForumIndexViewModel
    {
        public Pagination<ListableThreadViewModel> Pagination { get; set; }
        public string Topic { get; set; }
        public string[] Categories { get; set; }
        public List<Topic> TopicOptions { get; set; }
        public List<Category> CategoryOptions { get; set; }
    }
}
