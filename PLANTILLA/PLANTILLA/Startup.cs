using AspNetCore.SEOHelper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using PLANTILLA.Clases;
using PLANTILLA.Helpers;
using PLANTILLA.Services;

namespace PLANTILLA
{
    public class Startup
    {
        IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            
            HelperToken helper = new HelperToken(this.Configuration);
            services.AddAntiforgery();

            //DATA PROTECTION
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@"./"));

            services.AddTransient<Service_DEDOMENA>();

            services.AddMemoryCache();
            services.AddDistributedMemoryCache();
            services.AddHttpContextAccessor();

            services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddTransient<PathProvider>();

            services.AddTransient<DataManager>();
            services.AddTransient<SwitchEmpleo>();
            

            //session
            services.AddDistributedMemoryCache();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true; // consent required
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                options.Cookie.IsEssential = true;
                options.Cookie.MaxAge = TimeSpan.FromMinutes(15);
            });


            services.AddAuthentication(
               options =>
               {
                   options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                   options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                   options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
               }).AddCookie();
            services.AddControllersWithViews(options => options.EnableEndpointRouting = false);

            services.AddControllersWithViews();
            services.AddMvc();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Path.StartsWithSegments("/robots.txt"))
                {
                    var robotsTxtPath = Path.Combine(env.ContentRootPath, "robots.txt");
                    string output = "User-agent: *  \nallow: /";
                    if (File.Exists(robotsTxtPath))
                    {
                        output = await File.ReadAllTextAsync(robotsTxtPath);
                    }
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync(output);
                }
                else await next();
            });


            app.UseXMLSitemap(env.ContentRootPath);
            app.UseExceptionHandler("/Home/Error");
            //app.UseDeveloperExceptionPage();
            app.Use(async (ctx, next) =>
            {
                await next();

                if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                {
                    //Re-execute the request so the user gets the error page
                    string originalPath = ctx.Request.Path.Value;
                    ctx.Items["originalPath"] = originalPath;
                    ctx.Request.Path = "/Home/Error";
                    await next();
                }
            });

            // todo: replace with app.UseHsts(); once the feature will be stable
            //app.UseRewriter(new RewriteOptions().AddRedirectToHttps(StatusCodes.Status301MovedPermanently, 443));


            //app.UseHttpsRedirection();

            app.UseHsts();
            app.UseRouting();

            app.UseSession();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapRazorPages();
            });
        }
    }
}
