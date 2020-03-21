using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundaAPI.Models.ApiModels
{
    public class FundaResponseModel
    {
        public List<FundaObject> Objects { get; set; }
    }


    public class FundaObject
    {
        public string MakelaarNaam { get; set; }
    }
}
