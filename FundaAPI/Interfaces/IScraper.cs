using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundaAPI.Interfaces
{
    public interface IScraper
    {
        Task ScrapeObjects(int page = 1);

        Task ScrapeObjectsWithGarden(int page = 1);

        List<KeyValuePair<string, int>> GetMakelaarsWithPropertiesAmount();
        List<KeyValuePair<string, int>> GetMakelaarsWithPropertiesWithGardenAmount();

        bool GetPropertiesScrapingStatus();
        bool GetPropertiesWithGardenScrapingStatus();
    }
}
