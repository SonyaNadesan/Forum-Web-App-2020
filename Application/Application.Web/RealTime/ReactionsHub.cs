using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Application.Web.RealTime
{
    public class ReactionsHub : Hub
    {
        public async Task SendMessage(string recepientEmail)
        {
            await Clients.All.SendAsync("NotifyReaction", recepientEmail);
        }
    }
}
