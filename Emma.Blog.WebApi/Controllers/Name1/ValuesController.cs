using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Emma.Blog.WebApi.Extensions;
using Emma.Blog.WebApi.Models;

namespace Emma.Blog.WebApi.Controllers.Name1
{
   
    [Route("api/name1/[controller]")]
    public class ValuesController : Controller
    {
     
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "name1","value1", "value2" };
        }

    }
}
