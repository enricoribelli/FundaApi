using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FundaAPI.Interfaces;
using FundaAPI.Models.ApiModels;
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
        private readonly IApiCaller _apiCaller;
        private readonly IMapper _mapper;
        private readonly IOptions<ApiSettings> _apiSettings;


        public FundaApiController(IApiCaller apiCaller, IMapper mapper, IOptions<ApiSettings> apiSettings)
        {
            Requires.NotNull(mapper, nameof(mapper));
            Requires.NotNull(apiCaller, nameof(apiCaller));
            Requires.NotNull(apiSettings, nameof(apiSettings));

            _apiCaller = apiCaller;
            _mapper = mapper;
            _apiSettings = apiSettings;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = new FundaResponseViewModel();

            UriBuilder builder = new UriBuilder("http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/?type=koop&amp;zo=/amsterdam/&page=3600&pagesize=25")
            {
                
            };

            try
            {
                var fundaResult = await _apiCaller.GetResponseAsync<FundaResponseModel>(builder.Uri).ConfigureAwait(false);
                //if (fundaResult != null && fundaResult.Results.Any())
                //{
                //    //var fundaViewModel = new List<ItunesResponseViewModel>();
                //    //albumsApiResult.Results.ForEach(x => albumsViewModel.Add(_mapper.Map<ItunesResponseViewModel>(x)));
                //    //result.Albums = albumsViewModel.OrderBy(x => x.Title).ToList();
                //}
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, ex.Message);
            }

            //builder = new UriBuilder(_apiSettings.Value.GoogleApiUrl)
            //{ Query = $"?q={value}&maxResults={_apiSettings.Value.GoogleApiMaxResult}&printType=books&projection=lite" };

            //try
            //{
            //    var booksApiResult = await _apiCaller.GetResponseAsync<GoogleResponseModel>(builder.Uri).ConfigureAwait(false);
            //    if (booksApiResult != null && booksApiResult.Items.Any())
            //    {
            //        var booksViewModel = new List<GoogleResponseViewModel>();
            //        booksApiResult.Items.ForEach(x => booksViewModel.Add(_mapper.Map<GoogleResponseViewModel>(x.VolumeInfo)));
            //        result.Books = booksViewModel.OrderBy(x => x.Title).ToList();
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Log.Fatal(ex, ex.Message);

            //}

            return Ok(result);
        }
    }
}
