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


namespace Emma.Blog.WebApi
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

            var jwtSettings = new JwtSettings();
            Configuration.Bind("JwtSettings", jwtSettings);


            services.AddDbContext<BlogContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection"))

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
                            context.Token = context.HttpContext.Request.Cookies["JwtCookie"];
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

            app.UseAuthentication();
            app.UseMvc();

         

        }
    }
}
