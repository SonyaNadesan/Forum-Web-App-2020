﻿using Application.Domain.ApplicationEntities;
using Application.Services.Forum;
using Application.Services.Shared;
using Application.Web.ViewModels;
using Application.Web.ViewModels.ViewModelHelpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

            var resultsToDisplay = PaginationHelper.GetItemsToDisplay<Thread>(results, page, 5).ToList();

            var viewModel = new Pagination<Thread>()
            {
                CurrentPage = page,
                FormAction = "../Forum/Index",
                ItemsToDisplay = resultsToDisplay,
                PageSize = 5,
                StartPage = startPage,
                Query = query,
                TotalNumberOfResults = results.Count()
            };

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

                var topLevelPosts = _postService.GetTopLevelPosts(treadIdAsGuid).Result.ToList();

                var topLevelPostsForDisplay = PaginationHelper.GetItemsToDisplay<Post>(topLevelPosts, page, 10);

                var topLevelPostsAsViewModels = new List<PostWithRepliesViewModel>();

                foreach (var topLevelPost in topLevelPostsForDisplay)
                {
                    var allReplies = _postService.GetReplies(topLevelPost.Id).Result.ToList();
                    var repliesToDisplay = allReplies.Take(2).ToList();

                    var loadMoreViewModel = new LoadMoreViewModel<Post>()
                    {
                        Id = topLevelPost.Id.ToString(),
                        From = 0,
                        Take = repliesToDisplay.Count,
                        ItemsToDisplay = ViewModelHelper.Get(repliesToDisplay),
                        HasMore = allReplies.Count() > 2
                    };

                    var repliesViewModel = new PostWithRepliesViewModel()
                    {
                        TopLevelPost = ViewModelHelper.Get(topLevelPost),
                        Replies = loadMoreViewModel
                    };

                    topLevelPostsAsViewModels.Add(repliesViewModel);
                }

                var pagination = new PaginationWithId<PostWithRepliesViewModel>()
                {
                    Id = threadId,
                    NameOfIdFieldInView = "threadId",
                    CurrentPage = page,
                    FormAction = "../Forum/Thread",
                    ItemsToDisplay = topLevelPostsAsViewModels,
                    PageSize = 10,
                    Query = query,
                    StartPage = startPage,
                    TotalNumberOfResults = topLevelPosts.Count
                };

                var viewModel = new ViewModelWithPagination<Thread, PostWithRepliesViewModel>()
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

        public string GetRepliesOnPost(string postId, int from, int take)
        {
            var isPostIdGuid = Guid.TryParse(postId, out Guid postIdAsGuid);

            if (!isPostIdGuid)
            {
                throw new NullReferenceException();
            }

            var replies = _postService.GetReplies(postIdAsGuid).Result.ToList();
            var repliesToDisplay = replies.Skip(from).Take(take).ToList();
            var repliesAsViewModel = ViewModelHelper.Get(repliesToDisplay);

            if (!repliesAsViewModel.Any())
            {
                from = 1;
                repliesAsViewModel = replies.Skip(from).Take(take).ToList();
            }

            var numberOfItemsDiplayed = from + take;

            var loadMoreViewModel = new LoadMoreViewModel<Post>()
            {
                ItemsToDisplay = repliesAsViewModel,
                From = numberOfItemsDiplayed + 1,
                Take = take,
                Id = postId,
                HasMore = numberOfItemsDiplayed < replies.Count
            };

            var json = JsonConvert.SerializeObject(loadMoreViewModel, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });

            return json;
        }
    }
}
