using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scraper.Interfaces
{
    public interface IApiCaller
    {
        Task<T> GetResponseAsync<T>(Uri url);
    }
}
