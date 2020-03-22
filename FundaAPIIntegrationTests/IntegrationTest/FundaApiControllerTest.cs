using FundaAPI.BusinessServices;
using FundaAPI.Interfaces;
using FundaAPI.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FundaAPIIntegrationTests.IntegrationTest
{
    public class FundaApiControllerTest : FundaApiServiceIntegrationTestBase
    {

        private string ControllerPath => "api/FundaApi/GetMakelaarsWithPropertiesAmount";

        [Fact]
        public async Task GetApisResult_WithFullResponse()
        {
            await _scraper.ScrapeObjects();

            UriBuilder builder = new UriBuilder($"localhost/{ControllerPath}");

            HttpRequestMessage getRequest = new HttpRequestMessage(HttpMethod.Get, builder.Uri);

            var response = await _client.SendAsync(getRequest);
            Assert.True(response.IsSuccessStatusCode);

            Assert.NotNull(response.Content);

            var responseContent = await response.Content.ReadAsStringAsync();
            var deserializedResponse = JsonConvert.DeserializeObject<FundaResponseViewModel>(responseContent);

            Assert.True(deserializedResponse.IsScrapingComplete);
            Assert.Equal(2, deserializedResponse.MakelaarsWithPropertiesAmount.Count);
            Assert.Equal("TestMakelaar1", deserializedResponse.MakelaarsWithPropertiesAmount[0].Key);
            Assert.Equal(2, deserializedResponse.MakelaarsWithPropertiesAmount[0].Value);
            Assert.Equal("TestMakelaar2", deserializedResponse.MakelaarsWithPropertiesAmount[1].Key);
            Assert.Equal(1, deserializedResponse.MakelaarsWithPropertiesAmount[1].Value);
        }
    }
}
