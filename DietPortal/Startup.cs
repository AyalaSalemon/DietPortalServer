using BL;
using DL;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DietPortal
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
            services.AddDbContext<ProjectDBContext>(options => options.UseSqlServer(
               Configuration.GetConnectionString("OurDb")), ServiceLifetime.Scoped);
            services.AddAutoMapper(typeof(Startup));

            services.AddScoped<IUserInGroupBl, UserInGroupBl>();
            services.AddScoped<IUserInGroupDl, UserInGroupDl>();
            services.AddScoped<IRatingDl, RatingDl>();
            services.AddScoped<IRatingBl, RatingBl>();
            services.AddScoped<IUserDl, UserDl>();
            services.AddScoped<IUserBl, UserBl>();
            services.AddScoped<IGroupDl, GroupDl>();
            services.AddScoped<IGroupBl, GroupBl>();
            services.AddScoped<IPortalDl, PortalDl>();
            services.AddScoped<IPortalBl, PortalBl>();
            services.AddScoped<IWeightDl, WeightDl>();
            services.AddScoped<IWeightBl, WeightBl>();
            services.AddScoped<IMailBl, MailBl>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DietPortal", Version = "v1" });
            });
            services.AddResponseCaching();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.UseOurMiddleware();
         
            if (env.IsDevelopment())
            {
               
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DietPortal v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseResponseCaching();

            app.Use(async (context, next) =>
            {
                context.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        Public = true,
                        MaxAge = TimeSpan.FromSeconds(10)
                    };
                context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
                    new string[] { "Accept-Encoding" };

                await next();
            });

            //app.Map("/api", app1 =>
            //{
            //    app1.UseMiddleware1();
            //    app1.UseRouting();
            //    app1.UseAuthorization();

            //    app1.UseEndpoints(endpoints =>
            //    {
            //        endpoints.MapControllers();
            //    });
            //});

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
