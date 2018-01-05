using System.Collections.Generic;
using System.Security.Claims;

namespace Emma.Blog.Service.Auth
{
    public interface IClaimUser<TUser>
    {
        List<Claim> GetClaims();

        TUser GetUser();
    }
}
