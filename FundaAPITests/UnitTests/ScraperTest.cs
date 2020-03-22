using FundaAPI.BusinessServices;
using FundaAPI.Interfaces;
using FundaAPI.Models.ApiModels;
using FundaAPI.Options;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FundaAPITests.UnitTests
{
    public class ScraperTest
    {

        private readonly Mock<IApiCaller> _apiCaller;

        private readonly Scraper _target;


        public ScraperTest()
        {
            var apiSettings = Options.Create(new ApiSettings()
            {
                FundaUrl = "http://FundaApiWrongUrlTest"
            });

            _apiCaller = new Mock<IApiCaller>();
            _target = new Scraper(_apiCaller.Object, apiSettings);
        }

        [Theory]
        [InlineData("GetMakelaarsWithPropertiesAmount")]
        [InlineData("GetMakelaarsWithPropertiesWithGardenAmount")]
        public void GetMakelaarsWithPropertiesAmount_ShouldReturnWithCorrectAmountData(string methodName)
        {
            //Arrange
            var listOfMakelaars = new Dictionary<string, int>();
            listOfMakelaars.Add("TestMakelaar1", 1);
            listOfMakelaars.Add("TestMakelaar2", 10);
            listOfMakelaars.Add("TestMakelaar3", 1);
            listOfMakelaars.Add("TestMakelaar4", 20);
            listOfMakelaars.Add("TestMakelaar5", 1);
            listOfMakelaars.Add("TestMakelaar6", 30);
            listOfMakelaars.Add("TestMakelaar7", 1);
            listOfMakelaars.Add("TestMakelaar8", 40);
            listOfMakelaars.Add("TestMakelaar9", 1);
            listOfMakelaars.Add("TestMakelaar10", 50);
            listOfMakelaars.Add("TestMakelaar11", 1);
            listOfMakelaars.Add("TestMakelaar12", 60);

            _target.makelaarsWithPropertiesAmount = listOfMakelaars;
            _target.makelaarsWithPropertiesWithGardenAmount = listOfMakelaars;

            //Act
            Type scraperType = _target.GetType();
            MethodInfo scraperMethod = scraperType.GetMethod(methodName);
            var result = (List<KeyValuePair<string, int>>)scraperMethod.Invoke(_target, null);

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(10, result.Count);
            result.ForEach(x => Assert.Equal(x.Value, listOfMakelaars[x.Key]));
        }

        [Theory]
        [InlineData("ScrapeObjects")]
        [InlineData("ScrapeObjectsWithGarden")]
        public async void ScrapeObjects_ShouldSetCorrectData_UntilException(string methodName)
        {
            //Arrange
            var resultFromFundaApi = new FundaResponseModel
            {
                Objects = new List<FundaObject>() {
                    new FundaObject()
                    {
                        MakelaarNaam = "TestMakelaar1"
                    },
                    new FundaObject()
                    {
                        MakelaarNaam = "TestMakelaar1"
                    },
                    new FundaObject()
                    {
                        MakelaarNaam = "TestMakelaar2"
                    }
                }
            };

            _apiCaller.SetupSequence(x => x.GetResponseAsync<FundaResponseModel>(It.IsAny<Uri>()))
                .ReturnsAsync(resultFromFundaApi)
                .Throws(new Exception());

            //Act
            Type scraperType = _target.GetType();
            MethodInfo scraperMethod = scraperType.GetMethod(methodName);
            await (Task)scraperMethod.Invoke(_target, new object[] { It.IsAny<int>() });

            //Assert
            if(methodName == "ScrapeObjects")
            {
                Assert.Equal(2, _target.makelaarsWithPropertiesAmount.Count);
                Assert.False(_target.GetPropertiesScrapingStatus());
                Assert.Empty(_target.makelaarsWithPropertiesWithGardenAmount);
                Assert.Equal(2, _target.makelaarsWithPropertiesAmount["TestMakelaar1"]);
                Assert.Equal(1, _target.makelaarsWithPropertiesAmount["TestMakelaar2"]);
            }
            else
            {
                Assert.Equal(2, _target.makelaarsWithPropertiesWithGardenAmount.Count);
                Assert.False(_target.GetPropertiesWithGardenScrapingStatus());
                Assert.Equal(2, _target.makelaarsWithPropertiesWithGardenAmount["TestMakelaar1"]);
                Assert.Equal(1, _target.makelaarsWithPropertiesWithGardenAmount["TestMakelaar2"]);
            }
        }


        [Theory]
        [InlineData("ScrapeObjects")]
        [InlineData("ScrapeObjectsWithGarden")]
        public async void ScrapeObjects_ShouldSetCorrectData_WhenUnauthorizedResponse(string methodName)
        {
            //Arrange
            var resultFromFundaApi = new FundaResponseModel
            {
                Objects = new List<FundaObject>() {
                    new FundaObject()
                    {
                        MakelaarNaam = "TestMakelaar1"
                    },
                    new FundaObject()
                    {
                        MakelaarNaam = "TestMakelaar1"
                    },
                    new FundaObject()
                    {
                        MakelaarNaam = "TestMakelaar2"
                    }
                }
            };

            //This is a long running test, since I mocked the service to throw the "401 max number of requests reached exception"
            //and therefore my code waits 1 minute before doing the next call
            _apiCaller.SetupSequence(x => x.GetResponseAsync<FundaResponseModel>(It.IsAny<Uri>()))
                .ReturnsAsync(resultFromFundaApi)
                .Throws(new HttpRequestException("Response status code does not indicate success: 401 (Request limit exceeded)."))
                .ReturnsAsync(resultFromFundaApi)
                .ReturnsAsync(new FundaResponseModel() { Objects = new List<FundaObject>()}); //this is to make the scraper think it finished to scrape

            //Act
            Type scraperType = _target.GetType();
            MethodInfo scraperMethod = scraperType.GetMethod(methodName);
            await (Task)scraperMethod.Invoke(_target, new object[] { It.IsAny<int>() });

            //Assert
            if (methodName == "ScrapeObjects")
            {
                Assert.Equal(2, _target.makelaarsWithPropertiesAmount.Count);
                Assert.True(_target.GetPropertiesScrapingStatus());
                Assert.Empty(_target.makelaarsWithPropertiesWithGardenAmount);
                Assert.Equal(4, _target.makelaarsWithPropertiesAmount["TestMakelaar1"]);
                Assert.Equal(2, _target.makelaarsWithPropertiesAmount["TestMakelaar2"]);
            }
            else
            {
                Assert.Equal(2, _target.makelaarsWithPropertiesWithGardenAmount.Count);
                Assert.True(_target.GetPropertiesWithGardenScrapingStatus());
                Assert.Equal(4, _target.makelaarsWithPropertiesWithGardenAmount["TestMakelaar1"]);
                Assert.Equal(2, _target.makelaarsWithPropertiesWithGardenAmount["TestMakelaar2"]);
            }
        }

        [Theory]
        [InlineData("ScrapeObjects")]
        [InlineData("ScrapeObjectsWithGarden")]
        public async void ScrapeObjects_ShouldSetCorrectData_UntilScrapingIsFinished(string methodName)
        {
            //Arrange
            var resultFromFundaApi = new FundaResponseModel
            {
                Objects = new List<FundaObject>() {
                    new FundaObject()
                    {
                        MakelaarNaam = "TestMakelaar1"
                    },
                    new FundaObject()
                    {
                        MakelaarNaam = "TestMakelaar1"
                    },
                    new FundaObject()
                    {
                        MakelaarNaam = "TestMakelaar2"
                    }
                }
            };

            //This is a long running test, since I mocked the service to throw the "401 max number of requests reached exception"
            //and therefore my code waits 1 minute before doing the next call
            _apiCaller.SetupSequence(x => x.GetResponseAsync<FundaResponseModel>(It.IsAny<Uri>()))
                .ReturnsAsync(resultFromFundaApi)
                .ReturnsAsync(resultFromFundaApi)
                .ReturnsAsync(resultFromFundaApi)
                .ReturnsAsync(new FundaResponseModel() { Objects = new List<FundaObject>() }); //this is to make the scraper think it finished to scrape

            //Act
            Type scraperType = _target.GetType();
            MethodInfo scraperMethod = scraperType.GetMethod(methodName);
            await (Task)scraperMethod.Invoke(_target, new object[] { It.IsAny<int>() });


            //Assert

            if (methodName == "ScrapeObjects")
            {
                Assert.Equal(2, _target.makelaarsWithPropertiesAmount.Count);
                Assert.True(_target.GetPropertiesScrapingStatus());
                Assert.Empty(_target.makelaarsWithPropertiesWithGardenAmount);
                Assert.Equal(6, _target.makelaarsWithPropertiesAmount["TestMakelaar1"]);
                Assert.Equal(3, _target.makelaarsWithPropertiesAmount["TestMakelaar2"]);
            }
            else
            {
                Assert.Equal(2, _target.makelaarsWithPropertiesWithGardenAmount.Count);
                Assert.True(_target.GetPropertiesWithGardenScrapingStatus());
                Assert.Equal(6, _target.makelaarsWithPropertiesWithGardenAmount["TestMakelaar1"]);
                Assert.Equal(3, _target.makelaarsWithPropertiesWithGardenAmount["TestMakelaar2"]);
            }
        }


        [Fact]
        public async void ScrapeObjects_ShouldNotSetData_IfBeginningWithException()
        {

            _apiCaller.Setup(x => x.GetResponseAsync<FundaResponseModel>(It.IsAny<Uri>()))
                .Throws<Exception>();

            //Act
            await _target.ScrapeObjects(It.IsAny<int>());

            //Assert
            Assert.False(_target.GetPropertiesScrapingStatus());
            Assert.Empty(_target.makelaarsWithPropertiesWithGardenAmount);
            Assert.Empty(_target.makelaarsWithPropertiesAmount);
        }
    }
}
