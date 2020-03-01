using System.Web;

namespace TonieBox.Ui
{
    public static class Extensions
    {
        public static string GetParentPath(this string path) => path.Substring(0, path.LastIndexOf("/"));

        public static string EncodeUrl(this string value) => HttpUtility.UrlEncode(value);
        
        public static string DecodeUrl(this string value) => HttpUtility.UrlDecode(value);
    }
}
