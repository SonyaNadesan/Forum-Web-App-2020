using Application.Domain.ApplicationEntities;
using System.Collections.Generic;

namespace Application.Web.ViewModels
{
    public class RepliesViewModel
    {
        public Post Post { set; get; }
        public List<Post> Replies { get; set; }
    }
}
