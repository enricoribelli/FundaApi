using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Scraper.BusinessServices;
using Scraper.Interfaces;

namespace Scraper
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddTransient<IApiCaller, ApiCaller>();
                    services.AddSingleton<IScraperService, ScraperService>();

                    // Inject IHttpClientFactory
                    services.AddHttpClient();
                    services.AddHostedService<Worker>();
                });
    }
}
