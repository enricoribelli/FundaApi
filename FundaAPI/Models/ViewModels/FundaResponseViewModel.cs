using AutoMapper;
using FundaAPI.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundaAPI.Models.ViewModels
{
    public class FundaResponseViewModel
    {
        public List<KeyValuePair<string, int>> MakelaarsWithPropertiesAmount { get; set; }
        public bool IsScrapingComplete{ get; set; }
    }
}
