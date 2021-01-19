using Application.Data;
using Application.Domain.ApplicationEntities;
using System;
using System.Collections.Generic;

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
    }
}
