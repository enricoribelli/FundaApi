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

        private string ControllerPath => "api/FundaApi/GetMakelaarsWithPropertiesWithGardenAmount";
        private readonly IScraper _scraper;

        public FundaApiControllerTest()
        {
            _scraper = new Scraper(); ;
        }

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

            //Assert.Equal(deserializedResponse.Books.Count, _apiSettings.GoogleApiMaxResult);
            //Assert.Equal(deserializedResponse.Albums.Count, _apiSettings.ItunesApiMaxResult);
            //Assert.Equal(deserializedResponse.Books[0].Title, $"testBookTitle");
            //Assert.Equal(deserializedResponse.Books[0].Author, $"testAuthorName");
            //Assert.Equal(deserializedResponse.Albums[0].Title, $"testCollectionName");
            //Assert.Equal(deserializedResponse.Albums[0].Author, $"testArtistName");

        }
    }
}
