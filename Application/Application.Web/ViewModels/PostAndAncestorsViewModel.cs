using Application.Domain.ApplicationEntities;
using System.Collections.Generic;

namespace Application.Web.ViewModels
{
    public class PostAndAncestorsViewModel
    {
        public Post Post { get; set; }
        public List<Post> Ancestors { get; set; }
    }
}
