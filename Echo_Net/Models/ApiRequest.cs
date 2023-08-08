using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Echo_Net.Constants;

namespace Echo_Net.Models
{
    public class ApiRequest
    {
        public APIType ApiType { get; set;} = APIType.GET;
        public string? Url { get; set;}
        public object Data { get; set;} = APIType.GET;
        public string? AccessToken { get; set;}
        
    }
}