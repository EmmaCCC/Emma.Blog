using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Emma.Blog.Web.FileUpload.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UploadController : Controller
    {

        private IHostingEnvironment host;

        public UploadController(IHostingEnvironment host)
        {
            this.host = host;
        }

        [HttpPost("img")]
        public async Task<IActionResult> PostAsync(List<IFormFile> files)
        {
            try
            {
                var root = host.WebRootPath;
                var directory = "/images/" + DateTime.Now.ToString("yyyy/MM/dd/");
                string physicsPath = Path.Combine(root, directory);
                if (!Directory.Exists(physicsPath))
                {
                    Directory.CreateDirectory(physicsPath);
                }

                List<Task<string>> tasks = new List<Task<string>>(files.Count);

                foreach (var formFile in files)
                {
                    var file = formFile;
                    Task<string> task = Task.Run(async () =>
                    {
                        string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        string relativePath = Path.Combine(directory, fileName);
                        string path = root+ relativePath;

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        return relativePath;
                    });
                    tasks.Add(task);
                }
                await Task.WhenAll(tasks);
                List<string> paths = new List<string>(files.Count);

                foreach (var task in tasks)
                {
                    paths.Add(task.Result);
                }
                string domain = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;

                return Ok(new { status = 0, domain, paths });
            }
            catch (Exception ex)
            {
                return Ok(new { status = 1, message = "上传失败" });
            }

        }




    }
}
