using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using Newtonsoft.Json;
using Scraper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Scraper.BusinessServices
{
    public class ApiCaller : IApiCaller
    {
        private readonly IHttpClientFactory _httpFactory;

        public ApiCaller(IHttpClientFactory httpFactory)
        {
            Requires.NotNull(httpFactory, nameof(httpFactory));

            _httpFactory = httpFactory;
        }

        public async Task<T> GetResponseAsync<T>(Uri url)
        {
            using (HttpClient client = _httpFactory.CreateClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromSeconds(20);
                using (HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<T>(responseBody);
                }
            }
        }
    }
}
