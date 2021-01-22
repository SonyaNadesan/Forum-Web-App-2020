using Application.Domain.ApplicationEntities;
using System.Collections.Generic;

namespace Application.Web.ViewModels
{
    public class PostWithRepliesViewModel
    {
        public Post TopLevelPost { set; get; }
        public LoadMoreViewModel<Post> Replies { get; set; }
    }
}
