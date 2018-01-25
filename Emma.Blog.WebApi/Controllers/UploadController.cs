using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using Emma.Blog.WebApi.Extensions;
using Emma.Blog.WebApi.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Emma.Blog.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class UploadController : Controller
    {

        private IHostingEnvironment host;

        public UploadController(IHostingEnvironment host)
        {
            this.host = host;
        }
        [Authorize]
        [HttpPost("img")]
        public async Task<IActionResult> PostAsync(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = host.WebRootPath;

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    string path = Path.Combine(filePath, formFile.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, filePath });
            var name = HttpContext.User.Identity.Name;

            return Ok(new { status = 0, files });
        }




    }
}
