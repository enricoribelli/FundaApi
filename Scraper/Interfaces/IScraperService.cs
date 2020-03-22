using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scraper.Interfaces
{
    public interface IScraperService
    {
        Task ScrapeObjects();

        Task ScrapeObjectsWithGarden();

        Dictionary<string, int> GetMakelaarsWithPropertiesAmount();
    }
}
