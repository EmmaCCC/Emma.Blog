using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Emma.Blog.WebApi.Models
{
    public class UserViewModel
    {
        [Display(Name = "昵称")]
        [MinLength(6, ErrorMessage ="{0}不少于{1}个字符"),Required(ErrorMessage ="{0}是必填项")]
        public string NickName { get; set; }

        [Display(Name ="年龄")]
        [Range(1,5,ErrorMessage ="{0}必须在{1}到{2}岁之间"),Required(ErrorMessage = "{0}是必填项")]
        public int? Age { get; set; }
    }
}
