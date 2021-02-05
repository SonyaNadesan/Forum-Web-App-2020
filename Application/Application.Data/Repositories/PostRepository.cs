using Application.Domain.ApplicationEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Data.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext Context;

        public PostRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public Post Get(Guid postId)
        {
            return Context.Posts.Include(p => p.User).Include(p => p.Thread).Include(p => p.ParentPost).Include(p => p.ParentPost.User).Include(p => p.Thread.User).SingleOrDefault(p => p.Id == postId);
        }

        public IEnumerable<Post> GetAll()
        {
            return Context.Posts.Include(p => p.User).Include(p => p.Thread).Include(p => p.ParentPost);
        }

        public void Delete(Guid postId)
        {
            var post = Context.Posts.SingleOrDefault(p => p.Id == postId);

            if (post != null)
            {
                Context.Posts.Remove(post);
            }
        }

        public void Add(Post post)
        {
            var postFromDb = Get(post.Id);

            if (postFromDb == null)
            {
                Context.Entry(post.User).State = EntityState.Unchanged;
                Context.Entry(post.Thread).State = EntityState.Unchanged;

                if(post.ParentPost == null)
                {
                    post.ParentPost = post;
                }
                
                Context.Entry(post.ParentPost).State = EntityState.Unchanged;
                Context.Entry(post).State = EntityState.Added;
                Context.Posts.Add(post);
            }
        }

        public void Edit(Post post)
        {
            var result = Context.Posts.Include(p => p.User).SingleOrDefault(p => p.Id == post.Id);

            if (result != null)
            {
                result.User = post.User;
                result.Content = post.Content;
                result.DateTime = post.DateTime;
            }
        }
    }
}
