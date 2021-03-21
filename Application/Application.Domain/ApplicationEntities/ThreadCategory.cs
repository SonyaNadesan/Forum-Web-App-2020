using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Domain.ApplicationEntities
{
    public class ThreadCategory
    {
        public  Guid Id { get; set; }

        public Guid ThreadId { get; set; }

        [ForeignKey("ThreadId")]
        public virtual Thread Thread { get; set; }

        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
