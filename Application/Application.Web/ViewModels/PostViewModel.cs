using Application.Domain.ApplicationEntities;
using System;

namespace Application.Web.ViewModels
{
    public class PostViewModel
    {
        public Guid PostId { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }

        public string PostedBy { get; set; }

        public Guid ThreadId { get; set; }

        public Guid? ParentPostId { get; set; }

        public bool HasParentPost { get; set; }

        public int LevelInHierarchy { get; set; }

        public bool HasBeenViewedByThreadOwner { get; set; }

        public bool? HasBeenViewedByParentPostOwner { get; set; }

        public PostViewModel(Post post)
        {
            PostId = post.Id;
            Content = post.Content;
            DateTime = post.DateTime;
            ThreadId = post.ThreadId;
            ParentPostId = post.ParentPostId;
            HasParentPost = post.HasParentPost;
            LevelInHierarchy = post.LevelInHierarchy;
            HasBeenViewedByParentPostOwner = post.HasBeenViewedByParentPostOwner;
            HasBeenViewedByThreadOwner = post.HasBeenViewedByThreadOwner;
            PostedBy = post.User.FirstName + " " + post.User.LastName;
        }
    }
}
