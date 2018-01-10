using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Emma.Blog.Data.Models
{
    public class Post
    {
       
        public long PostId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreateTime { get; set; }

        public int ReadNumber { get; set; }

        public string Tag { get; set; }

        public long UserId { get; set; }

        public virtual User User { get; set; }
    }
}
