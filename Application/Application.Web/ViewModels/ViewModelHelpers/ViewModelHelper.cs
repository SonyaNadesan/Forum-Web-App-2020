using Application.Domain.ApplicationEntities;
using System.Collections.Generic;

namespace Application.Web.ViewModels.ViewModelHelpers
{
    public class ViewModelHelper
    {
        public static Post Get(Post post)
        {
            post.ParentPost = null;
            post.Thread = null;
            return post;
        }

        public static List<Post> Get(List<Post> posts)
        {
            foreach(var post in posts)
            {
                Get(post);
            }

            return posts;
        }
    }
}
