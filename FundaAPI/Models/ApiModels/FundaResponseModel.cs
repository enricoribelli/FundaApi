using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundaAPI.Models.ApiModels
{
    public class FundaResponseModel
    {
        public List<Object> Objects { get; set; }
        public Paging Paging { get; set; }
        public int TotaalAantalObjecten { get; set; }
    }

    public class Metadata
    {
        public string ObjectType { get; set; }
        public string Omschrijving { get; set; }
        public string Titel { get; set; }
    }


    public class Object
    {
       
        public List<object> ChildrenObjects { get; set; }
        
        public object IndTransactieMakelaarTonen { get; set; }
  
        public int MakelaarId { get; set; }
        public string MakelaarNaam { get; set; }
        
        public List<string> Producten { get; set; }
        public object ProjectNaam { get; set; }
        
        public List<int> ZoekType { get; set; }
    }

    public class Paging
    {
        public int AantalPaginas { get; set; }
        public int HuidigePagina { get; set; }
        public string VolgendeUrl { get; set; }
        public object VorigeUrl { get; set; }
    }
}
