using System;
using System.Collections.Generic;
using System.Security.Claims;
using Emma.Blog.Data.Enums;


namespace Emma.Blog.Data.Models
{
    public class Account
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public AccountType AccountType { get; set; }
    }
}
