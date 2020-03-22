using System;
using System.Collections.Generic;
using System.Text;

namespace Scraper.Models.ApiModels
{
    public class FundaResponseModel
    {
        public List<Object> Objects { get; set; }
    }


    public class Object
    {
        public string MakelaarNaam { get; set; }
    }
}
