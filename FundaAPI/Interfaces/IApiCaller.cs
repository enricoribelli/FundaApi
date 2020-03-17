using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FundaAPI.Interfaces
{
    public interface IApiCaller
    {
        Task<T> GetResponseAsync<T>(Uri url);
    }
}
