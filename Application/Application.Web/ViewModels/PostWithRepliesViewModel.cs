using Application.Domain.ApplicationEntities;

namespace Application.Web.ViewModels
{
    public class PostWithRepliesViewModel
    {
        public PostViewModel TopLevelPost { set; get; }
        public LoadMoreViewModel<PostViewModel> Replies { get; set; }
    }
}
