using Application.Domain.ApplicationEntities;
using System;
using System.Collections.Generic;

namespace Application.Web.ViewModels
{
    public class ReactionsByThreadViewModel
    {
        public Guid ThreadId { get; set; }

        public bool HasLoggedOnUserReactedToThread { get; set; }

        public List<SimpleUserViewModel> UsersWhoHaveReacted { get; set; }

        public User LoggedOnUser { get; set; }

        public ReactionsByThreadViewModel(Guid threadId, bool hasLoggedOnUserReactedToThread, List<SimpleUserViewModel> usersWhoHaveReacted, User loggedOnUser)
        {
            ThreadId = threadId;
            HasLoggedOnUserReactedToThread = hasLoggedOnUserReactedToThread;
            UsersWhoHaveReacted = usersWhoHaveReacted;
            LoggedOnUser = loggedOnUser;
        }
    }
}
