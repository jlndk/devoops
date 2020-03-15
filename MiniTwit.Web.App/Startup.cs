using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniTwit.Entities;
using MiniTwit.Models;
using Prometheus;

namespace MiniTwit.Web.App
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
            services.AddLetsEncrypt(o =>
            {
                o.DomainNames = new[] { "metamagicgames.com" };
                o.UseStagingServer = true; // <--- use staging

                o.AcceptTermsOfService = true;
                o.EmailAddress = "jooln@itu.dk";
            });
            services.AddControllersWithViews();
            services.AddDbContext<MiniTwitContext>();
            services.AddScoped<IMiniTwitContext>(provider => provider.GetService<MiniTwitContext>());
            services.AddIdentity<User, IdentityRole<int>>(options =>
                {
                    options.Lockout.MaxFailedAccessAttempts = 1000;
                    //perhaps we should reenable these at some point, or maybe just find a way to only disable them during development.
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 1;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.User.AllowedUserNameCharacters =
                        "abcdefghijklmnopqrstuvwxyzæøåABCDEFGHIJKLMNOPQRSTUVWXYZÆØÅ0123456789-._@+ ";
                })
                .AddEntityFrameworkStores<MiniTwitContext>()
                .AddDefaultTokenProviders();
            // TODO: This should perhaps be something other than scoped
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
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
                app.UseHsts();
            }
            
            app.UseMetricServer();
            app.UseHttpMetrics();

            app.UseStatusCodePages();
            app.UseStatusCodePagesWithReExecute("/StatusCode/Status{0}");
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
            });

            

            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            var context = serviceScope.ServiceProvider.GetRequiredService<MiniTwitContext>();
            context.Database.Migrate();
        }
    }
}
