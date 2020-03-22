using FundaAPI;
using FundaAPI.Interfaces;
using FundaAPI.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace FundaAPIIntegrationTests
{

   

    public class FundaApiServiceIntegrationTestBase
    {
        protected readonly HttpClient _client;
        protected readonly ApiSettings _apiSettings;

        public FundaApiServiceIntegrationTestBase()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            _apiSettings = config.GetSection("ApiSettings").Get<ApiSettings>();

            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .UseConfiguration(config);

            var server = new TestServer(builder);
            _client = server.CreateClient();

            //TODO: remove
            //var scraper = (IScraper)server.Services.GetService(typeof(IScraper));

            //scraper.ScrapeObjects();
        }
    }
}
