using Application.Domain.ApplicationEntities;
using System;
using System.Collections.Generic;

namespace Application.Services.Forum
{
    public interface IPostService
    {
        ServiceResponse<IEnumerable<Post>> GetAll();
        ServiceResponse<Post> Get(Guid postId);
        ServiceResponse<Post> Edit(Post Post);
        ServiceResponse<Post> Delete(Post Post);
        ServiceResponse<Post> Create(string email, string content, Guid threadId, Guid? parentPostId = null);
        ServiceResponse<IEnumerable<Post>> GetTopLevelPosts(Guid threadId);
        ServiceResponse<IEnumerable<Post>> GetReplies(Guid postId);
        ServiceResponse<IEnumerable<Post>> GetAncestors(Guid postId);
    }
}
