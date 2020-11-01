using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TicTacToe.Services.Concretes;
using TicTacToe.Services.Interfaces;
using TicTacToe.WebUI.Extensions;

namespace TicTacToe.WebUI
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
            services.AddRazorPages();
            services.AddSingleton<IUserService, UserService>();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            //app.Use(async (context, next) =>
            //{
            //    var url = context.Request.Path.Value;
            //    //context.Request.Path = "/Registration/UserRegistration/NewUser";
            //    //context.Response.Redirect("/Registration/UserRegistration/Index");
            //    if (url.Contains("/Registration/UserRegistration"))
            //    {
            //        context.Request.Path = "/Registration/UserRegistration/NewUser";
            //    }
            //    await next();
            //});
            var rewrite = new RewriteOptions().AddRewrite(
                 "Registration/UserRegistration/NewUser", "Registration/UserRegistration/index", true
                );
            app.UseRewriter(rewrite);
            app.UseHttpsRedirection();
            app.UseWebSockets();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });         

            app.UseCommunicationMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute
                (
                    name: "Registration",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute
                (
                     name: "default",
                     pattern: "{area=Registration}/{controller=Home}/{action=Index}/{id?}"
                 );
            });



  

        }
    }
}
