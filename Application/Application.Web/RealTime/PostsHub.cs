using Application.Services.Forum;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Application.Web.RealTime
{
    public class PostsHub : Hub
    {
        private readonly IPostService _postService;

        public PostsHub(IPostService postService)
        {
            _postService = postService;
        }

        public async Task SendMessage(string postId)
        {
            var post = _postService.Get(Guid.Parse(postId)).Result;

            var recipient = post.HasParent ? post.ParentPost.User.Id : post.Thread.User.Id;

            var postParentType = post.HasParent ? "POST" : "THREAD";

            await Clients.Users(recipient).SendAsync("NotifyUserOfPost", postParentType, post.Id, post.User.FirstName, post.Content, post.Thread.Id);
        }
    }
}
