using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Emma.Blog.Service.Auth
{
    public class JwtTokenUtil
    {
        public static string Encode(IEnumerable<Claim> claims, JwtSettings jwtSettings)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;
            var token = new JwtSecurityToken(
                jwtSettings.Issuer,
                jwtSettings.Audience,
                claims,
                now,
                now.AddSeconds(jwtSettings.Expires),
                creds
            );

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);

        }


        public static ClaimsPrincipal Decode(string token, JwtSettings jwtSettings, out SecurityToken validateToken)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var claimsPrincipal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero

                }, out validateToken);

                return claimsPrincipal;
            }
            catch (Exception ex)
            {
                validateToken = null;
                return null;
            }
        }
    }
}
