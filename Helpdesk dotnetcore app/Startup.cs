using AutoMapper;
using Helpdesk.Core.Interfaces;
using Helpdesk.Infrastructure.Data;
using Helpdesk.Infrastructure.Services.Repositories;
using Helpdesk_dotnetcore_app.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace Helpdesk_dotnetcore_app
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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                    options.SlidingExpiration = true;
                    options.AccessDeniedPath = "/Account/Login";
                });

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString
                ("defaultConnectionString")));
            services.AddControllersWithViews();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddAutoMapper(typeof(Program).Assembly);
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext db)
        {
            db.Database.Migrate();
            db.Seed();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                if (context.User != null && context.User.Claims.Any())
                {
                    var fullName = context.User.Claims.FirstOrDefault(c => c.Type == "FullName");
                    var username = context.User.Claims.FirstOrDefault(c => c.Type == "Username");
                    var id = context.User.Claims.FirstOrDefault(c => c.Type == "ID");

                    int.TryParse(id?.Value, out int idInt);

                    context.Session.SetString("FullName", fullName?.Value);
                    context.Session.SetString("Username", username?.Value);
                    context.Session.SetInt32("ID", idInt);
                }
                await next.Invoke();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Dashboard}/{action=Index}/{id?}");
            });
        }
    }
}
