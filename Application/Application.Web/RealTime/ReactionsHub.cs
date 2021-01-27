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

        public async Task SendMessage(string message)
        {
            var thread = _threadService.Get(Guid.Parse(message)).Result;

            var unseenReactions = thread.Reactions != null ? thread.Reactions.Where(r => !r.HasBeenViewedByThreadOwner).ToList() : new List<Reaction>();

            var jsonSerializerSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var result = JsonConvert.SerializeObject(unseenReactions, Formatting.None, jsonSerializerSettings);

            await Clients.All.SendAsync("NotifyReaction", result);
        }
    }
}
