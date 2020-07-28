using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Service.Interfaces;
using Service.Services;

namespace API
{
    public class Startup
    {
        public const string CookieAuthScheme = "CookieAuthScheme";
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthScheme)
            // Now add and configure cookie authentication
            .AddCookie(CookieAuthScheme, options =>
            {
                // Set the cookie name (optional)
                options.Cookie.Name = "AuthCookie";
                options.Cookie.Domain = "";
                // Set the samesite cookie parameter as none, 
                // otherwise it won’t work with clients on uses a different domain wont work!
                options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.None;
                // Simply return 401 responses when authentication fails 
                // as opposed to the default of redirecting to the login page
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = redirectContext =>
                    {
                        redirectContext.HttpContext.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddControllers();
            services.AddScoped<IAuthentication, Authentication>();


            services.AddScoped<DbContext, DatabaseContext>();

            var mappingConfig = new MapperConfiguration(mc =>   //
            {                                                   //
                mc.AddProfile(new MappingProfile());            // This adds the mapping profile class -- See below class
            });

            var mapper = mappingConfig.CreateMapper();          
                                                                
            services.AddSingleton(mapper);

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Application", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Application");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
