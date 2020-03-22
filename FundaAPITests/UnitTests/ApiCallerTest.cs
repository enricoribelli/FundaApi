using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using FundaAPI.BusinessServices;
using FundaAPI.Models.ApiModels;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;


namespace FundaAPITests.UnitTests
{
    public class ApiCallerTest
    {
        private readonly ApiCaller _target;
        private readonly Mock<IHttpClientFactory> _httpClientFactory;


        public ApiCallerTest()
        {
            _httpClientFactory = new Mock<IHttpClientFactory>();
            _target = new ApiCaller(_httpClientFactory.Object);
        }

        [Fact]
        public async void WhenFundaApiIsRunning_ServiceShouldReturn_WithCorrectData()
        {
            //Arrange
            var firstMakelaar = "Sinke Komejan Makelaars";
            var secondMakelaar = "Boss makelaardij";

            var mockHttp = new MockHttpMessageHandler();

            mockHttp.When("http://test")
                .Respond("application/json", $"{{'Objects':[{{'MakelaarNaam':'{firstMakelaar}'}},{{'MakelaarNaam':'{secondMakelaar}'}}]}}"); 

            var client = mockHttp.ToHttpClient();

            //Act
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);
            var response = await _target.GetResponseAsync<FundaResponseModel>(new Uri("http://test"));

            //Assert
            Assert.NotNull(response);
            Assert.NotEmpty(response.Objects);
            Assert.Equal(2, response.Objects.Count);
            Assert.Equal(response.Objects[0].MakelaarNaam, firstMakelaar);
            Assert.Equal(response.Objects[1].MakelaarNaam, secondMakelaar);
        }


        [Fact]
        public async void WhenResponseNotOk_ShouldThrowException()
        {
            //Arrange
            var mockHttp = new MockHttpMessageHandler();

            //Funda endpoint returns a 401 when more than 100 request per minutes are done
            mockHttp.When("http://test")
                .Respond(HttpStatusCode.Unauthorized, "application/json", String.Empty);

            var client = mockHttp.ToHttpClient();

            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);

            //Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _target.GetResponseAsync<FundaResponseModel>(new Uri("http://test")));
        }
    }
}
