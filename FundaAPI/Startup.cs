using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FundaAPI.BusinessServices;
using FundaAPI.Interfaces;
using FundaAPI.Options;
using GlobalExceptionHandler.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;

namespace FundaAPI
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
            services.AddControllers();

            //Adding Automapper
            services.AddAutoMapper(typeof(Startup));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Funda Coding Assignment",
                    Version = "v1",
                    Description = "API for retrieving info from Funda API"
                });
            });

            services.AddTransient<IApiCaller, ApiCaller>();
            services.AddSingleton<IScraper, Scraper>();

            // Inject IHttpClientFactory
            services.AddHttpClient();

            // Inject IOptions<T>
            services.AddOptions();
            var apiSettings = Configuration.GetSection("ApiSettings");
            services.Configure<ApiSettings>(apiSettings);
            var readyToUseApiSettings = apiSettings.Get<ApiSettings>();
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "Kramp Hub Assignment v1");
            });

            app.UseGlobalExceptionHandler(x =>
            {
                x.ContentType = "application/json";
                x.ResponseBody(_ => JsonConvert.SerializeObject(new
                {
                    Message = "An error occurred whilst processing your request"
                }));
            });
            app.Map("/error", x => x.Run(y => throw new Exception()));
        }
    }
}
