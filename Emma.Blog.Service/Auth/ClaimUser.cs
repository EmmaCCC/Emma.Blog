using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Emma.Blog.Data.Models;

namespace Emma.Blog.Service.Auth
{
    public class ClaimUser : IClaimUser<Account>
    {
        private readonly Account _account;
        public ClaimUser(Account account)
        {
            _account = account;
        }

        public ClaimUser()
        {
            _account = new Account();
        }
        public List<Claim> GetClaims()
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid,_account.Id.ToString()),
                new Claim(ClaimTypes.Name,_account.UserName),
                new Claim(ClaimTypes.Role, _account.AccountType.ToString()),
                
            };
            return claims;
        }


        public Account GetUser()
        {
            return _account;
        }
    }
}
