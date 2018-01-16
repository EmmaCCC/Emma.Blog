using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Emma.Blog.Data.Models;

namespace Emma.Blog.Service.Auth
{
    public class ClaimUser : IClaimUser<Data.Models.User>
    {
        private readonly Data.Models.User _account;
        public ClaimUser(Data.Models.User account)
        {
            _account = account;
        }

        public ClaimUser()
        {
            _account = new Data.Models.User();
        }
        public List<Claim> GetClaims()
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Sid,_account.UserId.ToString()),
                new Claim(ClaimTypes.Name,_account.UserName),
                
            };
            return claims;
        }


        public Data.Models.User GetUser()
        {
            return _account;
        }
    }
}
