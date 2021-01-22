using Application.Data;
using Application.Domain.ApplicationEntities;
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

            if(post == null)
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

            if(postFromDb == null)
            {
                response.ErrorMessage = "Post Not Found";
            }

            try
            {
                _unitOfWork.PostRepository.Edit(post);
            }
            catch(Exception ex)
            {
                response.ErrorMessage = "Sorry, something went wrong.";
            }

            return response;
        }

        public ServiceResponse<Post> Delete(Post post)
        {
            var postFromDb = _unitOfWork.PostRepository.Get(post.Id);

            var response = new ServiceResponse<Post>(postFromDb);

            if(postFromDb == null)
            {
                response.ErrorMessage = "Post Not Found.";
            }

            try
            {
                _unitOfWork.PostRepository.Delete(post.Id);
            }
            catch(Exception ex)
            {
                response.ErrorMessage = "Sorry, something went wrong.";
            }

            return response;
        }

        public ServiceResponse<Post> Create(string email, string content, Guid threadId, Guid? parentPostId)
        {
            var response = new ServiceResponse<Post>();

            var user = _unitOfWork.UserRepository.Get(email);

            if(user == null)
            {
                response.ErrorMessage = "User Not Set";
                return response;
            }

            var thread = _unitOfWork.ThreadRepository.Get(threadId);

            if(thread == null)
            {
                response.ErrorMessage = "Thread Not Set";
                return response;
            }

            var parentPost = parentPostId.HasValue ? _unitOfWork.PostRepository.Get(parentPostId.Value) : null;

            var newPost = new Post()
            {
                Id = Guid.NewGuid(),
                Content = content,
                ParentPostId = parentPost?.Id,
                ParentPost = parentPost,
                HasBeenViewedByParentPostOwner = false,
                HasBeenViewedByThreadOwner = false,
                Thread = thread,
                ThreadId = threadId,
                DateTime = DateTime.Now,
                UserId = user.Id,
                User = user,
                HasParentPost = parentPost != null
            };

            response.Result = newPost;

            try
            {
                _unitOfWork.PostRepository.Add(newPost);
                _unitOfWork.Save();
            }
            catch(Exception ex)
            {
                response.ErrorMessage = "Sorry, something went wrong.";
            }

            return response;
        }

        public ServiceResponse<IEnumerable<Post>> GetTopLevelPosts(Guid threadId)
        {
            var posts = _unitOfWork.PostRepository.GetAll().Where(p => p.ThreadId == threadId && !p.HasParentPost).ToList();

            return new ServiceResponse<IEnumerable<Post>>(posts);
        }

        public ServiceResponse<IEnumerable<Post>> GetReplies(Guid postId)
        {
            var post = _unitOfWork.PostRepository.Get(postId);

            var response = new ServiceResponse<IEnumerable<Post>>();

            if(post == null)
            {
                response.ErrorMessage = "Post Not Found.";
            }

            var repliesToDisplay = new List<Post>();

            var allPosts = _unitOfWork.PostRepository.GetAll().ToList();

            DrillDown(allPosts, post, repliesToDisplay);

            response.Result = repliesToDisplay;

            return response;
        }

        public ServiceResponse<IEnumerable<Post>> GetPostHierarchy(Guid threadId)
        {
            var response = new ServiceResponse<IEnumerable<Post>>();

            var posts = _unitOfWork.PostRepository.GetAll().Where(p => p.ThreadId == threadId && !p.HasParentPost).ToList();

            var allPostsInOrder = new List<Post>();

            foreach (var post in posts)
            {
                allPostsInOrder.Add(post);

                var replies = GetReplies(post.Id).Result.ToList();

                allPostsInOrder.AddRange(replies);
            }

            response.Result = allPostsInOrder;

            return response;
        }

        private void DrillDown(List<Post> allPosts, Post post, List<Post> results)
        {
            var replies = allPosts.Where(p => p.ThreadId == post.ThreadId && p.HasParentPost && p.ParentPostId == post.Id).ToList();

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
