using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TonieBox.Ui
{
    public static class Extensions
    {
        public static string GetParentPath(this string path) => path.Substring(0, path.LastIndexOf(System.IO.Path.DirectorySeparatorChar));

        public static string EncodeUrl(this string value) => HttpUtility.UrlEncode(value);
        
        public static string DecodeUrl(this string value) => HttpUtility.UrlDecode(value);
    }
}
