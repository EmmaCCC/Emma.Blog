using System;
using System.Collections.Generic;
using System.Text;

namespace Emma.Blog.Data.Models
{
    public class Post
    {
        public Guid PostId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreateTime { get; set; }

        public string Tag { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }
    }
}
