using System;
using System.Collections.Generic;
using System.Text;

namespace Emma.Blog.Data.Models
{
    public class Comment
    {
        public long CommentId { get; set; }

        public long UserId { get; set; }

        public string Content { get; set; }

        public string Name { get; set; }

        public DateTime CreateTime { get; set; }

        public int DeleteFlag { get; set; }

    }
}
