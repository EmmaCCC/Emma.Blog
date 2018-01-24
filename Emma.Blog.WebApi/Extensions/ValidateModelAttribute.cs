using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Emma.Blog.WebApi.Extensions
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            
            string msg = string.Empty;
            if (!modelState.IsValid)
            {
                var keys = modelState.Keys;
                foreach (var key in keys)
                {
                    if (modelState[key].Errors.Any())
                    {
                        msg = modelState[key].Errors.First().ErrorMessage;
                        break;
                    }
                }
               
                context.Result = new JsonResult(new { status = 1, message = msg });
            }


        }
    }
}
