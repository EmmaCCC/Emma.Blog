using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Emma.Blog.Data;
using Emma.Blog.Service;
using Emma.Blog.Service.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Emma.Blog.Common;
using System.Data;
using System.Reflection;
using System.Runtime.Loader;
using Emma.Blog.Model;
using Emma.Blog.Repository;

namespace Emma.Blog.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Global.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));

            var jwtSettings = new JwtSettings();
            Configuration.Bind("JwtSettings", jwtSettings);

            //ef
            services.AddDbContext<BlogContext>(options =>
                //options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection"))
                options.UseMySQL(Configuration.GetConnectionString("MySqlConnection"))
            );

         

            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(opts =>
                {
                    opts.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.HttpContext.Request.Cookies["token"];
                            return Task.CompletedTask;
                        }
                    };

                    opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                    };


                });

            services.AddCors();
            services.AddMvc();
            //dapper 注入
            services.AddScoped(typeof(IDbConnection), ConnectionFactory.SqlServerFactory);
            services.AddScoped(typeof(BaseRepository<>));

            var defaultLoader = AssemblyLoadContext.Default;

            //加载程序集
            var respository = defaultLoader.LoadFromAssemblyName(new AssemblyName("Emma.Blog.Repository"))
                .GetTypes().Where(a => a.GetCustomAttribute<InjectAttribute>() != null);
            var service = defaultLoader.LoadFromAssemblyName(new AssemblyName("Emma.Blog.Service"))
                .GetTypes().Where(a => a.GetCustomAttribute<InjectAttribute>() != null);

            //批量注入
            services.Scan(s => s.FromAssembliesOf(respository)
                .AddClasses().AsSelf().WithScopedLifetime());
            services.Scan(s => s.FromAssembliesOf(service)
                .AddClasses().AsSelf().WithScopedLifetime());


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider svp)
        {
            Service.ServiceProvider.Provider = svp;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            List<string> origins = new List<string>();
            Configuration.GetSection("Origins").Bind(origins);

            app.UseCors(opts =>
                opts.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .WithOrigins(origins.ToArray())
                    .SetPreflightMaxAge(TimeSpan.FromDays(60))
            );
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();

         

        }
    }
}
