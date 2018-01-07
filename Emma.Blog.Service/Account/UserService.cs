using System;
using Emma.Blog.Data;
using Emma.Blog.Data.Models;
using Emma.Blog.Service.Auth;
using Microsoft.EntityFrameworkCore;

namespace Emma.Blog.Service.Account
{
    public class UserService
    {
        protected DbContext dbContext = DbContextFactory.GetCurrentDbContext();
       
        public IClaimUser<User> Login(string username,string password)
        {
            return new ClaimUser(new User());
        }

        public IClaimUser<User> Register(User user)
        {

            user.UserId = Guid.NewGuid();
            user.CreateTime = DateTime.Now;
            dbContext.Add(user);
            dbContext.SaveChanges();
            return new ClaimUser(user);
        }
    }
}
