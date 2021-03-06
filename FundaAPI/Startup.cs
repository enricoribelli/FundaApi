using System;
using AutoMapper;
using FundaAPI.BusinessServices;
using FundaAPI.Interfaces;
using FundaAPI.Options;
using GlobalExceptionHandler.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace FundaAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

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

            services.AddHttpClient();

            services.AddOptions();
            var apiSettings = Configuration.GetSection("ApiSettings");
            services.Configure<ApiSettings>(apiSettings);
            var readyToUseApiSettings = apiSettings.Get<ApiSettings>();
        }

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
