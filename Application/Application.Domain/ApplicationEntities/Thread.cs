﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Domain.ApplicationEntities
{
    public class Thread
    {
        public Guid Id { get; set; }
        public string Heading { get; set; }
        public string Body { get; set; }
        public DateTime DateTime { get; set; }

        [Required]
        public string UserId { get; set; }

        public Guid? TopicId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("TopicId")]
        public virtual Topic Topic { get; set; }
        public virtual List<Post> Posts { get; set; }
        public virtual List<Reaction> Reactions { get; set; }
        public virtual List<Category> Categories { get; set; }
    }
}
