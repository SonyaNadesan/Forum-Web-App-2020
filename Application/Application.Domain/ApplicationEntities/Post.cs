using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Domain.ApplicationEntities
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public Guid ThreadId { get; set; }

        [ForeignKey("ThreadId")]
        public virtual Thread Thread { get; set; }

        public Guid? ParentPostId { get; set; }

        public Guid? TopLevelPostId { get; set; }

        public bool HasParentPost {get ; set;}

        [ForeignKey("ParentPostId")]
        public virtual Post ParentPost { get; set; }

        [ForeignKey("TopLevelPostId")]
        public virtual Post TopLevelPost { get; set; }

        public int LevelInHierarchy { get; set; }

        public bool HasBeenViewedByThreadOwner { get; set; }

        public bool? HasBeenViewedByParentPostOwner { get; set; }
    }
}
