using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using Scraper.Interfaces;
using Scraper.Models.ApiModels;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scraper.BusinessServices
{
    public class ScraperService : IScraperService
    {
        private readonly IApiCaller _apiCaller;
        private Dictionary<string, int> makelaarsWithPropertiesAmount;
        private Dictionary<string, int> makelaarsWithPropertiesWithGardenAmount;
        private bool isObjectSearchCompleted;
        private bool isObjectWithGardenSearchCompleted;



        public ScraperService(IApiCaller apiCaller)
        {
            Requires.NotNull(apiCaller, nameof(apiCaller));

            _apiCaller = apiCaller;
            makelaarsWithPropertiesAmount = new Dictionary<string, int>();
        }

        public Dictionary<string, int> GetMakelaarsWithPropertiesAmount()
        {
            return makelaarsWithPropertiesAmount;
        }

        public async Task ScrapeObjects()
        {
            var page = 2650;
            try
            {
                while (!isObjectSearchCompleted)
                {
                    UriBuilder builder = new UriBuilder("http://partnerapi.funda.nl/feeds/Aanbod.svc/json/ac1b0b1572524640a0ecc54de453ea9f/")
                    {
                        Query = $"?type=koop&amp;zo=/amsterdam/&page={page}&pagesize=25"
                    };

                    var fundaResult = await _apiCaller.GetResponseAsync<FundaResponseModel>(builder.Uri).ConfigureAwait(false);

                    if (fundaResult != null)
                    {
                        foreach (var obj in fundaResult.Objects)
                        {
                            UpdateDictionary(makelaarsWithPropertiesAmount, obj.MakelaarNaam);
                        }
                    }
                    if (!fundaResult.Objects.Any())
                    {
                        isObjectSearchCompleted = true;
                    }
                    else
                    {
                        page++;
                    }
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
                            UpdateDictionary(makelaarsWithPropertiesWithGardenAmount, obj.MakelaarNaam);
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
