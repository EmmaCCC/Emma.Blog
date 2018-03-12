using System;
using Emma.Blog.Data;
using Emma.Blog.Service;
using Emma.Blog.Service.Account;
using Emma.Blog.Service.Auth;
using Emma.Blog.Web.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Emma.Blog.Web.Models;
using System.Threading.Tasks;

namespace Emma.Blog.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            var jwt = new JwtSettings();
            Configuration.Bind("JwtSettings", jwt);

          
            services.AddMvc();
            services.AddDbContext<BlogContext>(options =>
            options.UseMySQL(Configuration.GetConnectionString("MySqlConnection"))
            //options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection"))
                );

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie((opts) =>
            {
                opts.LoginPath = "/User/Login";
                opts.Cookie.Name = "token";
                opts.TicketDataFormat = new JwtDataFormat(jwt);

            });

            services.AddScoped(typeof(UserService));

            //services.AddAuthorization();//授权，认可；批准，委任
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider svp)
        {

            Service.ServiceProvider.Provider = svp;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
            app.UseStaticFiles();
    
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
