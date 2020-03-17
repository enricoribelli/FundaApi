using FundaAPI.Interfaces;
using FundaAPI.Models.ApiModels;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundaAPI.BusinessServices
{
    

    public class Scraper
    {
        private readonly IApiCaller _apiCaller;
        private Dictionary<int, string> makelaars;



        public Scraper(IApiCaller apiCaller)
        {
            Requires.NotNull(apiCaller, nameof(apiCaller));

            _apiCaller = apiCaller;
            makelaars = new Dictionary<int, string>();
        }


        public async void ScrapeObjects()
        {
            UriBuilder builder = new UriBuilder("http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/?type=koop&amp;zo=/amsterdam/");


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

        }

        public async void ScrapeObjectsWithGarden()
        {

        }

        
    }
}
