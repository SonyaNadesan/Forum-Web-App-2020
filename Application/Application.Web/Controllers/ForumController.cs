using Application.Data;
using Application.Domain.ApplicationEntities;
using Application.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Application.Web.Controllers
{
    public class ForumController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ForumController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int page = 1, int startPage = 1, string query = "")
        {
            var allThreads = _unitOfWork.ThreadRepository.GetAll();

            var results = allThreads.OrderByDescending(t => t.DateTime);

            var viewModel = new Pagination<Thread>(results, page, 5, startPage, "../Forum/Index", query);

            return View(viewModel);
        }

        public IActionResult Thread(string threadId, int currentPage = 1, int startPage = 1, string query = "")
        {
            var isThreadIdGuid = Guid.TryParse(threadId, out Guid treadIdAsGuid);

            if (isThreadIdGuid)
            {
                var thread = _unitOfWork.ThreadRepository.Get(treadIdAsGuid);

                if (thread == null)
                {
                    return View("Index");
                }

                var posts = _unitOfWork.PostRepository.GetAll().Where(p => p.ThreadId == treadIdAsGuid);

                var page = new Pagination<Post>(posts, currentPage, 2, startPage, "../Forum/Thread", query);

                var viewModel = new ViewModelWithPagination<Thread, Post>()
                {
                    PageData = thread,
                    PaginationData = page
                };

                return View(viewModel);
            }


            return View("Index");
        }

        public IActionResult CreateThread()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult CreateThread(string heading, string body)
        {
            var currentUser = _unitOfWork.UserRepository.Get(User.Identity.Name);

            if(currentUser != null)
            {
                var newThread = new Thread()
                {
                    Id = Guid.NewGuid(),
                    Heading = heading,
                    Body = body,
                    DateTime = DateTime.Now,
                    User = currentUser,
                    UserId = currentUser.Id
                };

                _unitOfWork.ThreadRepository.Add(newThread);

                _unitOfWork.Save();
            }

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult CreatePost(string content, string threadId, string parentPostId = "")
        {
            var thread = Guid.TryParse(threadId, out Guid threadIdAsGuid) ? _unitOfWork.ThreadRepository.Get(threadIdAsGuid) : null;

            if (thread == null)
            {
                return RedirectToAction("Index");
            }

            var currentUser = _unitOfWork.UserRepository.Get(User.Identity.Name);

            var parentPost = Guid.TryParse(parentPostId, out Guid parentPostIdAsGuid) ? _unitOfWork.PostRepository.Get(parentPostIdAsGuid) : null;

            var newPost = new Post()
            {
                Id = Guid.NewGuid(),
                Content = content,
                ParentPostId = parentPostIdAsGuid,
                ParentPost = parentPost,
                HasBeenViewedByParentPostOwner = false,
                HasBeenViewedByThreadOwner = false,
                Thread = thread,
                ThreadId = threadIdAsGuid,
                DateTime = DateTime.Now,
                UserId = currentUser.Id,
                User = currentUser
            };

            _unitOfWork.PostRepository.Add(newPost);

            _unitOfWork.Save();

            return RedirectToAction("Index");
        }
    }
}
