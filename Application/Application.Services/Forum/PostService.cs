using Application.Data;
using Application.Domain.ApplicationEntities;
using Application.Services.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services.Forum
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ServiceResponse<IEnumerable<Post>> GetAll()
        {
            var posts = _unitOfWork.PostRepository.GetAll();

            var response = new ServiceResponse<IEnumerable<Post>>(posts);

            return response;
        }

        public ServiceResponse<Post> Get(Guid postId)
        {
            var post = _unitOfWork.PostRepository.Get(postId);

            var response = new ServiceResponse<Post>(post);

            if (post == null)
            {
                response.ErrorMessage = "Post Not Found";
                return response;
            }

            response.Result = post;
            return response;
        }

        public ServiceResponse<Post> Edit(Post post)
        {
            var postFromDb = _unitOfWork.PostRepository.Get(post.Id);

            var response = new ServiceResponse<Post>(postFromDb);

            if (postFromDb == null)
            {
                response.ErrorMessage = "Post Not Found";
            }

            try
            {
                _unitOfWork.PostRepository.Edit(post);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Sorry, something went wrong.";
            }

            return response;
        }

        public ServiceResponse<Post> Delete(Post post)
        {
            var postFromDb = _unitOfWork.PostRepository.Get(post.Id);

            var response = new ServiceResponse<Post>(postFromDb);

            if (postFromDb == null)
            {
                response.ErrorMessage = "Post Not Found.";
            }

            try
            {
                _unitOfWork.PostRepository.Delete(post.Id);
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Sorry, something went wrong.";
            }

            return response;
        }

        public ServiceResponse<Post> Create(string email, string content, Guid threadId, Guid? parentPostId)
        {
            var response = new ServiceResponse<Post>();

            var user = _unitOfWork.UserRepository.Get(email);

            if (user == null)
            {
                response.ErrorMessage = "User Not Set";
                return response;
            }

            var thread = _unitOfWork.ThreadRepository.Get(threadId);

            if (thread == null)
            {
                response.ErrorMessage = "Thread Not Set";
                return response;
            }

            var parentPost = parentPostId.HasValue ? _unitOfWork.PostRepository.Get(parentPostId.Value) : null;
            var newId = Guid.NewGuid();

            var newPost = new Post()
            {
                Id = newId,
                Content = content,
                ParentId = parentPost != null ? parentPost.Id : newId,
                ParentPost = parentPost,
                HasBeenViewedByParentPostOwner = false,
                HasBeenViewedByThreadOwner = false,
                Thread = thread,
                ThreadId = threadId,
                DateTime = DateTime.Now,
                UserId = user.Id,
                User = user,
                HasParent = parentPost != null,
                LevelInHierarchy = parentPost == null ? 1 : parentPost.LevelInHierarchy + 1
            };

            response.Result = newPost;

            try
            {
                _unitOfWork.PostRepository.Add(newPost);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Sorry, something went wrong.";
            }

            return response;
        }

        public ServiceResponse<IEnumerable<Post>> GetTopLevelPosts(Guid threadId)
        {
            var posts = _unitOfWork.PostRepository.GetAll().Where(p => p.ThreadId == threadId && !p.HasParent).ToList();

            return new ServiceResponse<IEnumerable<Post>>(posts);
        }

        public ServiceResponse<IEnumerable<Post>> GetReplies(Guid postId)
        {
            var post = _unitOfWork.PostRepository.Get(postId);

            var response = new ServiceResponse<IEnumerable<Post>>();

            if (post == null)
            {
                response.ErrorMessage = "Post Not Found.";
            }

            var allPosts = _unitOfWork.PostRepository.GetAll().ToList();

            response.Result = new FlattenHierarchyService<Post, Guid>(allPosts).GetDescendants(post, (x, y) => x == y);

            return response;
        }

        public ServiceResponse<IEnumerable<Post>> GetAncestors(Guid postId)
        {
            var response = new ServiceResponse<IEnumerable<Post>>();

            var post = _unitOfWork.PostRepository.Get(postId);

            var allPostsInOrder = new List<Post>();

            while (post != null && post.HasParent)
            {
                allPostsInOrder.Add(post);

                post = post?.ParentPost;
            }

            response.Result = allPostsInOrder.Reverse<Post>();

            return response;
        }

        private void DrillDown(List<Post> allPosts, Post post, List<Post> results)
        {
            var replies = allPosts.Where(p => p.ThreadId == post.ThreadId && p.HasParent && p.ParentId == post.Id).ToList();

            var drillDown = replies.Any();

            while (drillDown)
            {
                foreach (var reply in replies)
                {
                    results.Add(reply);

                    DrillDown(allPosts, reply, results);
                }

                drillDown = false;
            }
        }
    }
}
