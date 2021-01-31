using Application.Services.Forum;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Application.Domain.ApplicationEntities;
using Application.Services.UserProfile;

namespace Application.Web.RealTime
{
    public class ReactionsHub : Hub
    {
        private readonly IThreadService _threadService;
        private readonly IReactionService _reactionService;

        public ReactionsHub(IThreadService threadService, IReactionService reactionService)
        {
            _threadService = threadService;
            _reactionService = reactionService;
        }

        public async Task SendMessage(string threadId, string senderUserId, int totalNotifications)
        {
            //var threadIdAsGuid = Guid.Parse(threadId);

            //var userIdOfThreadOwner = _threadService.Get(threadIdAsGuid).Result.UserId;

            //var reaction = _reactionService.GetByUserId(threadIdAsGuid, senderUserId);

            //var result = new JsonResult(new { reaction, threadId, totalNotifications });

            //result.StatusCode = 200;

            //result.ContentType = "application/json";

            await Clients.Caller.SendAsync("NotifyReaction", totalNotifications);
        }
    }
}
