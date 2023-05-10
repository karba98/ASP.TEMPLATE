using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using PLANTIILLA.DEDOMENA.Data;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.PlatformAbstractions;
using System.Reflection;
using System.IO;
using PLANTIILLA.DEDOMENA.Services;
using PLANTIILLA.DEDOMENA.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using PLANTIILLA.DEDOMENA.Repositories;
using PLANTIILLA.DEDOMENA.Clases;
using Quartz;
using Quartz.Spi;
using Quartz.Impl;

namespace PLANTIILLA.DEDOMENA.Helpers
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        static string XmlCommentsFilePath
        {
            get
            {
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var fileName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name + ".xml";
                return Path.Combine(basePath, fileName);
            }
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //ICARO
            string url_icaro = Configuration.GetConnectionString("VYPPORTAL.ICARO");


            string cadenasql =
                Configuration.GetConnectionString("DbConnect");

            services.AddControllers();
            services.AddTransient<HelperToken>();

            HelperToken helper = new HelperToken(Configuration);
            services.AddDbContextPool<DataContext>(
                opt =>
                opt.UseMySql(cadenasql,
                ServerVersion.AutoDetect(cadenasql))
            );

            //Injection
            services.AddTransient(options => new Service_ICARO(url_icaro));
            services.AddTransient<FileService>();
            services.AddTransient<EmpleoUpdater>();
            services.AddTransient<PathProvider>();
            services.AddTransient<Service_INFOJOBS>();
            services.AddTransient<Service_INDEED>();
            services.AddTransient<Service_JOOBLE>();
            services.AddTransient<Service_VYP_NOTICIAS>();
            services.AddTransient<Service_VYP_CURSOS>();
            services.AddTransient<Service_VYPW_RSS>();
            services.AddTransient<DataManager>();
            services.AddTransient<SwitchEmpleo>();
            services.AddTransient<RepositoryEmpresas>();
            services.AddTransient<RepositoryEmpleo>();
            services.AddTransient<RepositoryEmpleoBR>();
            services.AddTransient<RepositoryWords>();
            services.AddTransient<RepositoryDocuments>();
            services.AddTransient<RepositoryUsers>();
            
            services.AddTransient<EmpleoUpdater>();
            services.AddHostedService<QuartzHostedService>();

            // Add the required Quartz.NET services
            services.AddQuartz(q =>
            {
                // Use a Scoped container to create jobs. I'll touch on this later
                q.UseMicrosoftDependencyInjectionScopedJobFactory();
            });

            // Add the Quartz.NET hosted service

            services.AddQuartzHostedService(
                q => q.WaitForJobsToComplete = true);
            // Add our job
            services.AddTransient<Job>();
            services.AddTransient(options => new JobSchedule(
                jobType: typeof(Job),
                //cronExpression: "0 0/1 * 1/1 * ? *")); // run every 1 minute
                cronExpression: "0 0 0/1 1/1 * ? *")); // run every 1 hour
                //cronExpression: "0 0 0/2 1/1 * ? *")); // run every 2 hour

            // Add Quartz services
            services.AddTransient<IJobFactory, JobFactory>();
            services.AddTransient<ISchedulerFactory, StdSchedulerFactory>();



            //Swagger
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "PLANTIILLA - DEDOMENA",
                    Description = "API de datos del Portal Vigilancia y Proteccion",
                    //TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Vigilancia y proteccion",
                        Url = new Uri("https://vigilanciayproteccion.website")
                    },
                    //License = new OpenApiLicense
                    //{
                    //    Name = "Example License",
                    //    Url = new Uri("https://example.com/license")
                    //}
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
                options.IncludeXmlComments(XmlCommentsFilePath);

            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15);
                options.Cookie.IsEssential = true;
            });

            services.AddAuthentication(
               helper.GetAuthOptions()).AddJwtBearer(helper.GetJwtBearerOptions()).AddCookie();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("./swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
