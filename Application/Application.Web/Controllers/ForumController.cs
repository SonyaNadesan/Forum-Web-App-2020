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

        public IActionResult Index(int currentPage = 1)
        {
            var allThreads = _unitOfWork.ThreadRepository.GetAll();

            var page = new PaginationViewModel<Thread>()
            {
                ItemsToDisplay = allThreads.Take(5).ToList(),
                CurrentPage = currentPage,
                PageSize = 2,
                TotalNumberOfResults = allThreads.Count()
            };

            return View(page);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult CreateThread(string heading, string body)
        {
            var currentUser = _unitOfWork.UserRepository.Get(User.Identity.Name);

            var newThread = new Thread()
            {
                Id = Guid.NewGuid(),
                Heading = heading,
                Body = body,
                DateTime = DateTime.Now,
                UserId = currentUser.Id,
                User = currentUser
            };

            _unitOfWork.ThreadRepository.Add(newThread);

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult CreatePost(string content, string body, string threadId, string? parentPostId = "")
        {
            var isThreadIdGuid = Guid.TryParse(threadId, out Guid threadIdAsGuid);

            var thread = isThreadIdGuid ? _unitOfWork.ThreadRepository.Get(threadIdAsGuid) : null;

            var currentUser = _unitOfWork.UserRepository.Get(User.Identity.Name);

            var isParentPostIdGuid = Guid.TryParse(parentPostId, out Guid parentPostIdAsGuid);

            var parentPost = isParentPostIdGuid ? _unitOfWork.PostRepository.Get(parentPostIdAsGuid) : null;

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

            _unitOfWork.ThreadRepository.Add(newThread);

            return RedirectToAction("Index");
        }
    }
}
