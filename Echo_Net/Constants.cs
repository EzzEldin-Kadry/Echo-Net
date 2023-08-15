using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.SignalR;

namespace Echo_Net
{
    public static class Constants
    {
        private const string FormatDateTimeStamp = "yyyy_MM_dd__HH_mm_ss_ffff";
        public const string EchoesLocation = "data\\echos";
        public static string AudioPostBase { get; set; } = "";
        public enum APIType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
        public static string DateTimeStamp()
        {
            var pOut = DateTime.Now.ToString(FormatDateTimeStamp);
            return pOut;
        }
    }
}