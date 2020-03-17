using FundaAPI.Interfaces;
using FundaAPI.Models.ApiModels;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundaAPI.BusinessServices
{
    

    public class Scraper : IScraper
    {
        private readonly IApiCaller _apiCaller;
        private Dictionary<string, int> makelaarsProperties;
        private Dictionary<string, int> makelaarsPropertiesWithGarden;
        private bool isObjectSearchCompleted;
        private bool isObjectWithGardenSearchCompleted;



        public Scraper(IApiCaller apiCaller)
        {
            Requires.NotNull(apiCaller, nameof(apiCaller));

            _apiCaller = apiCaller;
            makelaarsProperties = new Dictionary<string, int>();
        }


        public async Task ScrapeObjects()
        {
            UriBuilder builder = new UriBuilder("http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/?type=koop&amp;zo=/amsterdam/&page=2600&pagesize=25");
            try
            {
                while (!isObjectSearchCompleted)
                {
                    var fundaResult = await _apiCaller.GetResponseAsync<FundaResponseModel>(builder.Uri).ConfigureAwait(false);

                    if (fundaResult != null)
                    {
                        foreach(var obj in fundaResult.Objects)
                        {
                            UpdateDictionary(makelaarsProperties, obj.MakelaarNaam);
                        }
                    }
                    if (!fundaResult.Objects.Any()) isObjectSearchCompleted = true;
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, ex.Message);
            }

        }

        public async Task ScrapeObjectsWithGarden()
        {
            UriBuilder builder = new UriBuilder("http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/?type=koop&amp;zo=/amsterdam/tuin/&page=2600&pagesize=25");
            try
            {
                while (!isObjectWithGardenSearchCompleted)
                {
                    var fundaResult = await _apiCaller.GetResponseAsync<FundaResponseModel>(builder.Uri).ConfigureAwait(false);

                    if (fundaResult != null)
                    {
                        foreach (var obj in fundaResult.Objects)
                        {
                            UpdateDictionary(makelaarsPropertiesWithGarden, obj.MakelaarNaam);
                        }
                    }
                    if (!fundaResult.Objects.Any()) isObjectSearchCompleted = true;
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, ex.Message);
            }

        }


        private void UpdateDictionary(Dictionary<string, int> dict, string key)
        {
            if (dict.ContainsKey(key))
            {
                dict[key]++;
            }
            else
            {
                dict.Add(key, 0);
            }
        }

        
    }
}
