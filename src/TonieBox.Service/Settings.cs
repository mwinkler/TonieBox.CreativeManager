using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TonieBox.Service
{
    public class Settings
    {
        public IEnumerable<string> SupportedFileExtensions { get; set; }
        public string LibraryRoot { get; set; }
    }
}
