using Application.Domain.ApplicationEntities;
using Application.Services.Forum;
using Application.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Application.Web.Controllers
{
    public class ForumController : Controller
    {
        private readonly IThreadService _threadService;
        private readonly IPostService _postService;

        public ForumController(IThreadService threadService, IPostService postService)
        {
            _threadService = threadService;
            _postService = postService;
        }

        public IActionResult Index(int page = 1, int startPage = 1, string query = "")
        {
            var allThreads = _threadService.GetAll();

            var results = allThreads.Result.OrderByDescending(t => t.DateTime);

            var viewModel = new Pagination<Thread>(results, page, 5, startPage, "../Forum/Index", query);

            return View(viewModel);
        }

        public IActionResult Thread(string threadId, int page = 1, int startPage = 1, string query = "")
        {
            var isThreadIdGuid = Guid.TryParse(threadId, out Guid treadIdAsGuid);

            if (isThreadIdGuid)
            {
                var thread = _threadService.Get(treadIdAsGuid);

                if (!thread.IsValid)
                {
                    return View("Index");
                }

                var posts = _postService.GetAll().Result.Where(p => p.ThreadId == treadIdAsGuid && !p.HasParentPost);

                var pagination = new PaginationWithId<Post>(posts, page, 2, startPage, "../Forum/Thread", query)
                {
                    Id = threadId,
                    NameOfIdFieldInView = "threadId"
                };

                var viewModel = new ViewModelWithPagination<Thread, Post>()
                {
                    PageData = thread.Result,
                    PaginationData = pagination
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
            var createThreadResponse = _threadService.Create(User.Identity.Name, heading, body);

            if (!createThreadResponse.IsValid)
            {
                return RedirectToAction("Index"); //Needs changing
            }

            return RedirectToAction("Index");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult CreatePost(string content, string threadId, string parentPostId = "")
        {
            var threadIsGuid = Guid.TryParse(threadId, out Guid threadIdAsGuid);

            if (!threadIsGuid)
            {
                return RedirectToAction("Index");
            }

            Guid.TryParse(parentPostId, out Guid parentPostIdAsGuid);

            var postCreationResponse = _postService.Create(User.Identity.Name, content, threadIdAsGuid, parentPostIdAsGuid);

            if (postCreationResponse.IsValid)
            {
                return RedirectToAction("Thread", new { threadId = threadId });
            }

            return RedirectToAction("Index");
        }

        public IActionResult GetRepliedOnPost(string postId)
        {
           
        }
    }
}
