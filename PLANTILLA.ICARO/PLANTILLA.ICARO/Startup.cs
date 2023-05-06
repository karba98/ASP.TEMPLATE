using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;

using PLANTILLA.ICARO.Clases;
using PLANTILLA.ICARO.Helpers;
using PLANTILLA.ICARO.Data;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.PlatformAbstractions;
using System.Reflection;
using System.IO;

namespace PLANTILLA.ICARO
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
            String cadenasql =
                this.Configuration.GetConnectionString("DbConnect");

            services.AddControllers();
            services.AddTransient<Repositories.RepositoryUsers>();
            services.AddTransient<Repositories.RepositoryTokens>();
            services.AddTransient<HelperToken>();
            HelperToken helper = new HelperToken(Configuration);
            services.AddAuthentication(helper.GetAuthOptions()).AddJwtBearer(helper.GetJwtBearerOptions()).AddCookie();
            services.AddDbContextPool<DataContext>(
                opt =>
                opt.UseMySql(cadenasql,
                ServerVersion.AutoDetect(cadenasql))
            );    

            services.AddTransient<FacebookPoster>();
            services.AddTransient<TelegramPoster>();
            services.AddTransient<TwitterPoster>();
            services.AddTransient<LinkedinPoster>();

            services.AddSwaggerGen(options =>
            {
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
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "VYPPORTAL - ICARO",
                    Description = "Sandbox para acciones de publicación en redes sociales del Portal Vigilancia y Proteccion",
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
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {

            }
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("./swagger/v1/swagger.json", "v1");
                options.RoutePrefix = String.Empty;
            });
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
