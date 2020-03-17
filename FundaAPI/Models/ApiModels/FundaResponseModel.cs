using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundaAPI.Models.ApiModels
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
