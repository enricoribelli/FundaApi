using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FundaAPI.Interfaces;
using FundaAPI.Models.ApiModels;
using FundaAPI.Models.ViewModels;
using FundaAPI.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using Serilog;

namespace FundaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FundaApiController : ControllerBase
    {
        private readonly IScraper _scraper;


        public FundaApiController(IScraper scraper)
        {
            Requires.NotNull(scraper, nameof(scraper));
            _scraper = scraper;
        }


        [HttpGet]
        [Route("GetMakelaarsWithPropertiesAmount")]
        public IActionResult GetMakelaarsWithPropertiesAmount()
        {
            var result = new FundaResponseViewModel();

            var orderedMakelaars = _scraper.GetMakelaarsWithPropertiesAmount();
            result.MakelaarsWithPropertiesAmount = orderedMakelaars;
            result.IsScrapingComplete = _scraper.GetPropertiesScrapingStatus();

            return Ok(result);
        }

        [HttpGet]
        [Route("GetMakelaarsWithPropertiesWithGardenAmount")]
        public IActionResult GetMakelaarsWithPropertiesWithGardenAmount()
        {
            var result = new FundaResponseViewModel();

            var orderedMakelaars = _scraper.GetMakelaarsWithPropertiesWithGardenAmount();
            result.MakelaarsWithPropertiesAmount = orderedMakelaars;
            result.IsScrapingComplete = _scraper.GetPropertiesWithGardenScrapingStatus();

            return Ok(result);
        }
    }
}
