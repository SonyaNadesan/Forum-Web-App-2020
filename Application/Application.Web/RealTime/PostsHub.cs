using Application.Services.Forum;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Application.Web.RealTime
{
    public class PostsHub : Hub
    {
        private readonly IPostService _postService;
        private readonly IThreadService _threadService;

        public PostsHub(IPostService postService, IThreadService threadService)
        {
            _postService = postService;
            _threadService = threadService;
        }

        public async Task SendMessage(string postId, string threadId, string senderUserId)
        {
            var post = _postService.Get(Guid.Parse(postId)).Result;

            var recipient = post.HasParentPost ? post.ParentPost.User.Id : post.Thread.User.Id;

            var postParentType = post.HasParentPost ? "POST" : "THREAD";

            await Clients.Users(recipient).SendAsync("NotifyUserOfPost", postParentType, post.Id, post.User.FirstName, post.Content, post.Thread.Id);
        }
    }
}
