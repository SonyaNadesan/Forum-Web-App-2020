using Application.Domain;
using Application.Domain.ApplicationEntities;
using Application.Services.Filtering;
using Application.Services.Forum;
using Application.Services.Forum.Filters;
using Application.Services.Shared;
using Application.Services.UserProfile;
using Application.Web.ViewModels;
using Application.Web.ViewModels.ViewModelHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Web.Controllers
{
    [Authorize]
    public class ForumController : Controller
    {
        private readonly IThreadService _threadService;
        private readonly IPostService _postService;
        private readonly IReactionService _reactionService;
        private readonly IUserProfileService _userProfileService;
        private readonly ICategoryService _categoryService;
        private readonly ITopicService _topicService;
        private readonly IThreadFilterBuilder _threadFilterBuilder;
        private readonly IFilterService<Thread> _threadFilterService;

        public ForumController(IThreadService threadService, IPostService postService, IReactionService reactionService,
                               IUserProfileService userProfileService, ICategoryService categoryService, ITopicService topicService,
                               IThreadFilterBuilder threadFilterBuilder, IFilterService<Thread> threadFilterService)
        {
            _threadService = threadService;
            _postService = postService;
            _reactionService = reactionService;
            _userProfileService = userProfileService;
            _categoryService = categoryService;
            _topicService = topicService;
            _threadFilterBuilder = threadFilterBuilder;
            _threadFilterService = threadFilterService;
        }

        public IActionResult Index(int page = 1, int startPage = 1, string query = "", string topic = "", string categories = "", Enums.MatchConditions matchCondition = Enums.MatchConditions.MatchAny)
        {
            topic = string.IsNullOrEmpty(topic) ? "all" : topic;

            categories = string.IsNullOrEmpty(categories) ? "all" : categories;

            var categoryCollection = CollectionGenerationFromQueryParamService.GenerateCollection<HashSet<string>>(categories, "all", '+');

            var allThreads = _threadService.GetAll().Result.ToList();

            var filters = _threadFilterBuilder.AddTopicFilter(topic)
                                              .AddCategoryFilter(categoryCollection, matchCondition)
                                              .Build();

            var results = _threadFilterService.GetFilteredList(allThreads, filters).OrderByDescending(t => t.DateTime);

            var resultsToDisplay = PaginationHelper.GetItemsToDisplay(results, page, 5).ToList();

            var pagination = new Pagination<Thread>()
            {
                CurrentPage = page,
                FormAction = "../Forum/Index",
                ItemsToDisplay = resultsToDisplay,
                PageSize = 5,
                StartPage = startPage,
                Query = query,
                TotalNumberOfResults = results.Count()
            };

            var viewModel = new ForumIndexViewModel()
            {
                Pagination = pagination,
                Categories = categoryCollection.ToArray(),
                Topic = topic,
                CategoryOptions = _categoryService.GetAll().Result.ToList(),
                TopicOptions = _topicService.GetAll().Result.ToList()
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
                        From = 0 + 2,
                        Take = 2,
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
            var allCategories = _categoryService.GetAll().Result.ToList();
            var allTopics = _topicService.GetAll().Result.ToList();

            var viewModel = new CreateThreadViewModel()
            {
                Heading = string.Empty,
                Categories = { },
                Body = string.Empty,
                Topic = string.Empty,
                CategoryOptions = allCategories,
                TopicOptions = allTopics
            };

            return View(viewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult CreateThread(CreateThreadViewModel viewModel)
        {
            var selectedTopic = _topicService.GetAll().Result.SingleOrDefault(x => x.NameInUrl == viewModel.Topic.ToLower());
            var selectedCategories = _categoryService.GetAll().Result.Where(x => viewModel.Categories.Contains(x.NameInUrl)).ToList();

            var createThreadResponse = _threadService.Create(User.Identity.Name, viewModel.Heading, viewModel.Body, selectedTopic, selectedCategories);

            if (createThreadResponse.IsValid)
            {
                var allCategories = _categoryService.GetAll().Result.ToList();
                var allTopics = _topicService.GetAll().Result.ToList();

                viewModel.CategoryOptions = allCategories;
                viewModel.TopicOptions = allTopics;

                return View(viewModel);
            }

            return RedirectToAction("CreateThread");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public JsonResult CreatePost([FromBody] CreatePostViewModel createPostViewModel)
        {
            var threadIsGuid = Guid.TryParse(createPostViewModel.threadId, out Guid threadIdAsGuid);

            if (!threadIsGuid)
            {
                throw new NullReferenceException();
            }

            Guid.TryParse(createPostViewModel.parentPostId, out Guid parentPostIdAsGuid);

            var postCreationResponse = _postService.Create(User.Identity.Name, createPostViewModel.content, threadIdAsGuid, parentPostIdAsGuid);

            if (postCreationResponse.IsValid)
            {
                var json = JsonConvert.SerializeObject(postCreationResponse.Result, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                return new JsonResult(JsonConvert.DeserializeObject<Post>(json));
            }

            throw new Exception();
        }

        public JsonResult GetRepliesOnPost(string postId, int from, int take)
        {
            var isPostIdGuid = Guid.TryParse(postId, out Guid postIdAsGuid);

            if (!isPostIdGuid)
            {
                throw new NullReferenceException();
            }

            var replies = _postService.GetReplies(postIdAsGuid).Result.ToList();
            var repliesToDisplay = replies.Skip(from).Take(take).ToList();
            var repliesAsViewModel = ViewModelHelper.Get(repliesToDisplay);

            var numberOfItemsDiplayed = from + take;

            var loadMoreViewModel = new LoadMoreViewModel<Post>()
            {
                ItemsToDisplay = repliesAsViewModel,
                From = numberOfItemsDiplayed,
                Take = take,
                Id = postId,
                HasMore = numberOfItemsDiplayed < replies.Count
            };

            var json = JsonConvert.SerializeObject(loadMoreViewModel, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            });

            return new JsonResult(JsonConvert.DeserializeObject<LoadMoreViewModel<Post>>(json));
        }

        public JsonResult GetReactions(string threadId, int returnCount = 3)
        {
            var threadIdAsGuid = Guid.Parse(threadId);

            var user = _userProfileService.Get(User.Identity.Name);

            if (!user.IsValid)
            {
                throw new NullReferenceException();
            }

            var reactions = _reactionService.GetReactionsByThreadId(threadIdAsGuid);

            var hasUserReacted = reactions.Any(r => r.User.Id == user.Result.Id && r.ThreadId == threadIdAsGuid);

            returnCount = hasUserReacted ? returnCount - 1 : returnCount;

            returnCount = returnCount >= 0 ? returnCount : 0;

            var usersWhoHaveReactedViewModel = reactions.Where(r => r.UserId != user.Result.Id).Take(returnCount).Select(r => new SimpleUserViewModel(r.User)).ToList();

            var viewModel = new ReactionsByThreadViewModel()
            {
                ThreadId = threadIdAsGuid,
                HasLoggedOnUserReactedToThread = hasUserReacted,
                UsersWhoHaveReacted = usersWhoHaveReactedViewModel,
                LoggedOnUser = new SimpleUserViewModel(user.Result),
                TotalReactions = reactions.Count()
            };

            return new JsonResult(viewModel);
        }

        [HttpPost]
        public JsonResult UpdateReactions([FromBody] string threadId)
        {
            if (!string.IsNullOrEmpty(threadId))
            {
                _reactionService.Respond(User.Identity.Name, Guid.Parse(threadId));

                var viewModel = GetReactions(threadId);

                return new JsonResult(viewModel);
            }

            throw new NullReferenceException();
        }

        [HttpGet]
        public JsonResult IndividualPost(string postId)
        {
            var postIdAsGuid = Guid.Parse(postId);

            var post = _postService.Get(postIdAsGuid).Result;

            if (post.HasParentPost && !post.HasBeenViewedByParentPostOwner.Value && post.ParentPost.User.Email == User.Identity.Name)
            {
                post.HasBeenViewedByParentPostOwner = true;
            }
            else if (!post.HasBeenViewedByThreadOwner && User.Identity.Name == post.Thread.User.Email)
            {
                post.HasBeenViewedByThreadOwner = true;
            }

            _postService.Edit(post);

            var ancestors = _postService.GetAncestors(postIdAsGuid).Result;

            var result = new PostAndAncestorsViewModel()
            {
                Post = post,
                Ancestors = ancestors.ToList()
            };

            var json = JsonConvert.SerializeObject(result, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return new JsonResult(JsonConvert.DeserializeObject<PostAndAncestorsViewModel>(json));
        }
    }
}
