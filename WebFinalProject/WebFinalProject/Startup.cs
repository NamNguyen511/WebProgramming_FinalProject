using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebFinalProject.Data;
using WebFinalProject.Models;

namespace WebFinalProject
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
            services.AddControllersWithViews();
            services.AddDbContext<UTEUniversityContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //AddIdentity  add cookie based authentication
            // Add scoped class fors things like UserMangager, SignInManager
            // Note :
            
            services.AddIdentity<IdentityUser, IdentityRole>
                ( 
                    //options => { options.SignIn.RequireConfirmedEmail = true; }
                ).AddEntityFrameworkStores<UTEUniversityContext>().AddDefaultTokenProviders();

            // Make password policy
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
            });
            services.AddMvcCore(options =>
           {
               var policy = new AuthorizationPolicyBuilder()
                               .RequireAuthenticatedUser().
                               Build();
               options.Filters.Add(new AuthorizeFilter(policy));
           });
            /*services.ConfigureApplicationCookie(options =>
            {
                options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
                {
                    OnRedirectToReturnUrl = ctx =>
                    {
                        var requestPath = ctx.Request.Path;
                        if (requestPath.Value == "/Student/Create")
                        {
                            ctx.Response.Redirect("/Account/Login");
                        }
                        return Task.CompletedTask;
                    }
                };
            });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

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

            app.UseRouting();
         
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=UTEUniversity}/{action=Index}/{id?}");
            });
        }
    }
}
