using Application.Services.Forum;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Application.Domain.ApplicationEntities;

namespace Application.Web.RealTime
{
    public class ReactionsHub : Hub
    {
        private readonly IThreadService _threadService;

        public ReactionsHub(IThreadService threadService)
        {
            _threadService = threadService;
        }

        public async Task SendMessage(string message, string recipientUserId)
        {
            var thread = _threadService.Get(Guid.Parse(message)).Result;

            var unseenReactions = thread.Reactions != null ? thread.Reactions.Where(r => !r.HasBeenViewedByThreadOwner).ToList() : new List<Reaction>();

            var reactionsForNotification = unseenReactions.Select(x => new { x.ThreadId, x.Thread.Heading, x.ReactionType, x.User.FirstName, x.Thread.User.Email });

            var result = new JsonResult(reactionsForNotification);

            await Clients.User(recipientUserId).SendAsync("NotifyReaction", result);
        }
    }
}
