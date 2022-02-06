using Sonya.AspNetCore.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Domain.ApplicationEntities
{
    public class Post : IHierarchyItem<Guid, Guid>
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

        public Guid ParentId { get; set; }

        public bool HasParent {get ; set;}

        [ForeignKey("ParentId")]
        public virtual Post ParentPost { get; set; }

        public int LevelInHierarchy { get; set; }

        public bool HasBeenViewedByThreadOwner { get; set; }

        public bool? HasBeenViewedByParentPostOwner { get; set; }
    }
}
