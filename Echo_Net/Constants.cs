using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Echo_Net
{
    public static class Constants
    {
        public static string AudioPostBase { get; set; }
        public enum APIType
        {
            GET,
            POST,
            PUT,
            DELETE
        } 
    }
}