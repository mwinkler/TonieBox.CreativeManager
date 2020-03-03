using System;
using System.Web;

namespace TonieCreativeManager.Service
{
    public static class Extensions
    {
        public static string GetParentPath(this string path) => path.Substring(0, Math.Max(path.LastIndexOf("/"), 0));

        public static string EncodeUrl(this string value) => HttpUtility.UrlEncode(value);
        
        public static string DecodeUrl(this string value) => HttpUtility.UrlDecode(value);
    }
}
