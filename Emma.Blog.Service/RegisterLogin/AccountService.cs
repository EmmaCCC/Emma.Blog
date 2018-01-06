using System;
using System.Collections.Generic;
using System.Text;
using Emma.Blog.Data;
using Emma.Blog.Data.Models;
using Emma.Blog.Service.Auth;

namespace Emma.Blog.Service.RegisterLogin
{
    public class AccountService
    {
        protected BlogContext dbContext;
        public AccountService(BlogContext context)
        {
            dbContext = context;
        }
        public IClaimUser<Account> Login(string username,string password)
        {
            Account account = new Account
            {
                Id = Guid.NewGuid(),
                UserName = "songlinuser",
                AccountType = Data.Enums.AccountType.SuperAdmin
            };

            return new ClaimUser(account);
        }
    }
}
