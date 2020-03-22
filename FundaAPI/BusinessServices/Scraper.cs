using FundaAPI.Interfaces;
using FundaAPI.Models.ApiModels;
using FundaAPI.Options;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FundaAPI.BusinessServices
{
    //Long time running service started at startup of our API that scrapes the external Funda Api
    public class Scraper : IScraper
    {
        private readonly IApiCaller _apiCaller;
        public Dictionary<string, int> makelaarsWithPropertiesAmount;
        public Dictionary<string, int> makelaarsWithPropertiesWithGardenAmount;
        private bool isObjectSearchCompleted;
        private bool isObjectWithGardenSearchCompleted;
        private readonly IOptions<ApiSettings> _apiSettings;



        public Scraper(IApiCaller apiCaller, IOptions<ApiSettings> apiSettings)
        {
            _apiSettings = apiSettings;
            _apiCaller = apiCaller;
            makelaarsWithPropertiesAmount = new Dictionary<string, int>();
            makelaarsWithPropertiesWithGardenAmount = new Dictionary<string, int>();
        }

        //Scrapes properties without garden
        public async Task ScrapeObjects(int page)
        {
            try
            {
                while (!isObjectSearchCompleted)
                {
                    UriBuilder builder = new UriBuilder(_apiSettings.Value.FundaUrl)
                    {
                        Query = $"?type=koop&amp;zo=/amsterdam/&page={page}&pagesize=25"
                    };
                    var fundaResult = await _apiCaller.GetResponseAsync<FundaResponseModel>(builder.Uri).ConfigureAwait(false);
                    await Task.Delay(1300);//to be on the safe side I wait a little bit longer than needed

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
            catch (HttpRequestException ex)
            {
                Log.Error(ex, ex.Message);

                //we could have other kinds of http exception
                if (ex.Message == "Response status code does not indicate success: 401 (Request limit exceeded).")
                {
                    Thread.Sleep(TimeSpan.FromSeconds(60));
                    await ScrapeObjects(page);
                }
                
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, ex.Message);
            }

        }

        //Scrapes properties with garden
        public async Task ScrapeObjectsWithGarden(int page)
        {
            try
            {
                while (!isObjectWithGardenSearchCompleted)
                {
                    UriBuilder builder = new UriBuilder(_apiSettings.Value.FundaUrl)
                    {
                        Query = $"?type=koop&amp;zo=/amsterdam/tuin/&page={page}&pagesize=25"
                    };
                    var fundaResult = await _apiCaller.GetResponseAsync<FundaResponseModel>(builder.Uri).ConfigureAwait(false);
                    await Task.Delay(1300);//to be on the safe side I wait a little bit longer than needed

                    if (fundaResult != null)
                    {
                        foreach (var obj in fundaResult.Objects)
                        {
                            UpdateDictionary(makelaarsWithPropertiesWithGardenAmount, obj.MakelaarNaam);
                        }
                    }
                    if (!fundaResult.Objects.Any())
                    {
                        isObjectWithGardenSearchCompleted = true;
                    }
                    else
                    {
                        page++;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Log.Error(ex, ex.Message);

                //we could have other kinds of http exception
                if (ex.Message == "Response status code does not indicate success: 401 (Request limit exceeded).")
                {
                    Thread.Sleep(TimeSpan.FromSeconds(60));
                    await ScrapeObjectsWithGarden(page);
                }
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, ex.Message);
            }

        }

        public bool GetPropertiesScrapingStatus()
        {
            return isObjectSearchCompleted;
        }

        public bool GetPropertiesWithGardenScrapingStatus()
        {
            return isObjectWithGardenSearchCompleted;
        }

        public List<KeyValuePair<string, int>> GetMakelaarsWithPropertiesAmount()
        {
            var sum = 0;
            makelaarsWithPropertiesAmount.ToList().ForEach(x => sum = sum + x.Value);


            var sortedMakelaars = makelaarsWithPropertiesAmount.OrderByDescending(x => x.Value);

            //out of all the scraped results, the 10 with the highest amount of properties must be returned
            return sortedMakelaars.Take(10).ToList();
        }

        public List<KeyValuePair<string, int>> GetMakelaarsWithPropertiesWithGardenAmount()
        {
            var sortedMakelaars = makelaarsWithPropertiesWithGardenAmount.OrderByDescending(x => x.Value);

            //out of all the scraped results, the 10 with the highest amount of properties must be returned
            return sortedMakelaars.Take(10).ToList();
        }


        private void UpdateDictionary(Dictionary<string, int> dict, string key)
        {
            if (dict.ContainsKey(key))
            {
                dict[key]++;
            }
            else
            {
                dict.Add(key, 1);
            }
        }


    }
}
