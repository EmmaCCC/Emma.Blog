using System;
using System.Collections.Generic;
using System.Linq;
using Emma.Blog.Common;
using Emma.Blog.Data;
using Emma.Blog.Data.Models;
using Emma.Blog.Service.Auth;
using Microsoft.EntityFrameworkCore;

namespace Emma.Blog.Service.Account
{
    public class UserService
    {
        protected DbContext context = DbContextFactory.GetCurrentDbContext();

        public IClaimUser<User> Login(string username, string password)
        {
            return new ClaimUser(new User());
        }

        public IClaimUser<User> Register(User user)
        {

            user.UserId = SnowId.NewId();
            user.CreateTime = DateTime.Now;
            context.Add(user);
            context.SaveChanges();
            return new ClaimUser(user);
        }

        public List<User> GetPageList(int pageIndex, int pageSize)
        {
            var query = context.PageAsQuery<User, DateTime>(pageIndex, pageSize, out int totalCount, out int pageCount, u => true, true, u => u.CreateTime);

            var list = query.ToList();
            return list;
        }


        public bool Delete(long id)
        {

            var user = context.Find<User>(id);

            if (user != null)
            {
                context.Remove(user);
            }

            return context.SaveChanges() > 0;
        }
    }
}
