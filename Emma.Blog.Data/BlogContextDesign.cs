using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Emma.Blog.Data
{
    public class BlogContextDesign : IDesignTimeDbContextFactory<BlogContext>
    {
        public BlogContext CreateDbContext(string[] args)
        {
            Directory.SetCurrentDirectory("..");//设置当前路径为当前解决方案的路径
            string appSettingBasePath = Directory.GetCurrentDirectory() + "/Emma.Blog.Web";//改成你的appsettings.json所在的项目名称

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(appSettingBasePath)
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<BlogContext>();
   
            builder.UseMySQL(configBuilder.GetConnectionString("MySqlConnection"));
            return new BlogContext(builder.Options);
          
        }
    }
}
