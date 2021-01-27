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

        public int TotalReactions { get; set; }
    }
}
