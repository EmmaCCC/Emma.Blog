using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Emma.Blog.Service.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Emma.Blog.WebApi.Extensions
{
    public class CaptchaValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //最终办法还是使用https 防止截获
            var request = context.HttpContext.Request;
            string clientId = request.Cookies["clientId"];
            string code = request.Form["code"];
            if (string.IsNullOrEmpty(clientId))
            {
                context.Result = new UnauthorizedResult();
            }
            if (!ValidateCode.ContainsClientId(clientId))
            {
                context.Result = new UnauthorizedResult();
            }
            //验证是否需要验证码
            if (ValidateCode.IsRequired(clientId))
            {
                if (!ValidateCode.Check(clientId, code))
                {
                    context.Result = new JsonResult(new { status = 1, message = "验证码不正确" });
                }
            }

        }
    }
}
