using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Emma.Blog.WebApi.Extensions
{
    public class SignatureRequiredAttribute : ActionFilterAttribute
    {
        private string _key = "21y39hkhsdia";
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //最终办法还是使用https 防止截获
            string timestamp = context.HttpContext.Request.Query["timestamp"];
            string signature1 = context.HttpContext.Request.Query["signature"];
            string nonce = context.HttpContext.Request.Query["nonce"];
            if (string.IsNullOrEmpty(signature1))
            {
                context.Result = new UnauthorizedResult();
            }
            //拼串
            string input = _key + timestamp + nonce;

            //防止重放攻击 把时间戳 存放到缓存中（做个静态类） 如果重复 说明是重放签名 则不予受理
            //因为客户端每次请求时间戳肯定是不一样的
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            string signature2 = BitConverter.ToString(sha1.ComputeHash(Encoding.UTF8.GetBytes(input)));
            signature2 = signature2.Replace("-", "").ToLower();

            //验证
            if (signature2 != signature1)
            {
                context.Result = new UnauthorizedResult();
            }


        }
    }
}
