using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace WineAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(AppContext.BaseDirectory))
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", buildOptions => { buildOptions.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
            });

            services.AddSingleton(Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAllOrigins");

            app.UseMvc();
        }
    }
}
