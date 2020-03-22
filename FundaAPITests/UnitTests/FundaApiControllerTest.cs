using FundaAPI.BusinessServices;
using FundaAPI.Controllers;
using FundaAPI.Interfaces;
using FundaAPI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FundaAPITests.UnitTests
{
    public class FundaApiControllerTest
    {
        private readonly Mock<IScraper> _scraper;
        private readonly FundaApiController _target;


        public FundaApiControllerTest()
        {
            _scraper = new Mock<IScraper>();

            _target = new FundaApiController(_scraper.Object);
        }

        [Fact]
        public void WhenScraperIsRunning_GetMakelaarsWithPropertiesAmount_ShouldReturnOkStatus_WithCorrectData()
        {
            //Arrange

            var listOfMakelaars = new List<KeyValuePair<string, int>>();
            listOfMakelaars.Add(new KeyValuePair<string, int>("TestMakelaar1", 3));
            listOfMakelaars.Add(new KeyValuePair<string, int>("TestMakelaar2", 1));


            _scraper.Setup(x => x.GetMakelaarsWithPropertiesAmount())
                .Returns(listOfMakelaars);

            //Act
            var result = _target.GetMakelaarsWithPropertiesAmount();

            //Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value as FundaResponseViewModel;
            Assert.NotNull(model);

            Assert.Equal(model.MakelaarsWithPropertiesAmount.Count, listOfMakelaars.Count);
            Assert.Equal(model.MakelaarsWithPropertiesAmount[0].Key, listOfMakelaars[0].Key);
            Assert.Equal(model.MakelaarsWithPropertiesAmount[0].Value, listOfMakelaars[0].Value);

            Assert.Equal(model.MakelaarsWithPropertiesAmount[1].Key, listOfMakelaars[1].Key);
            Assert.Equal(model.MakelaarsWithPropertiesAmount[1].Value, listOfMakelaars[1].Value);
        }

        
        [Fact]
        public void WhenScraperIsRunning_GetMakelaarsWithPropertiesWithGardenAmount_ShouldReturnOkStatus_WithCorrectData()
        {
            //Arrange

            var listOfMakelaars = new List<KeyValuePair<string, int>>();
            listOfMakelaars.Add(new KeyValuePair<string, int>("TestMakelaar1", 3));
            listOfMakelaars.Add(new KeyValuePair<string, int>("TestMakelaar2", 1));


            _scraper.Setup(x => x.GetMakelaarsWithPropertiesWithGardenAmount())
                .Returns(listOfMakelaars);

            //Act
            var result = _target.GetMakelaarsWithPropertiesWithGardenAmount();

            //Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);

            var model = okObjectResult.Value as FundaResponseViewModel;
            Assert.NotNull(model);

            Assert.Equal(model.MakelaarsWithPropertiesAmount.Count, listOfMakelaars.Count);
            Assert.Equal(model.MakelaarsWithPropertiesAmount[0].Key, listOfMakelaars[0].Key);
            Assert.Equal(model.MakelaarsWithPropertiesAmount[0].Value, listOfMakelaars[0].Value);

            Assert.Equal(model.MakelaarsWithPropertiesAmount[1].Key, listOfMakelaars[1].Key);
            Assert.Equal(model.MakelaarsWithPropertiesAmount[1].Value, listOfMakelaars[1].Value);
        }
    }
}
