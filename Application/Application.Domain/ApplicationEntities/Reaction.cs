using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Application.Domain.Enums;

namespace Application.Domain.ApplicationEntities
{
    public class Reaction
    {
        public Guid ReactionId { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [Required]
        public Guid ThreadId { get; set; }

        [ForeignKey("ThreadId")]
        public virtual Thread Thread { get; set; }

        public string ReactionType { get; set; }

        public bool HasBeenViewedByThreadOwner { get; set; }

        public Reaction()
        {
            ReactionType = Enum.GetName(typeof(ReactionTypes), ReactionTypes.NONE);
        }

        public Reaction(Guid reactionId, User user, Thread thread, ReactionTypes reactionType, bool hasBeenViewedByThreadOwner = false)
        {
            ReactionId = reactionId;
            User = user;
            UserId = user.Id;
            Thread = thread;
            ThreadId = thread.Id;
            ReactionType = Enum.GetName(typeof(ReactionTypes), reactionType);
            HasBeenViewedByThreadOwner = hasBeenViewedByThreadOwner;
        }

        public Reaction(User user, Thread thread, ReactionTypes reactionType)
        {
            ReactionId = Guid.NewGuid();
            User = user;
            UserId = user.Id;
            Thread = thread;
            ThreadId = thread.Id;
            ReactionType = Enum.GetName(typeof(ReactionTypes), reactionType);
        }

    }
}
