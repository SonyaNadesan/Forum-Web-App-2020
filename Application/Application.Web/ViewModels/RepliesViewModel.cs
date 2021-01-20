using Application.Domain.ApplicationEntities;
using System.Collections.Generic;

namespace Application.Web.ViewModels
{
    public class RepliesViewModel
    {
        public Post TopLevelPost { set; get; }
        public List<Post> Replies { get; set; }
    }
}
