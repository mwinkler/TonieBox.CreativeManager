using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TonieBox.Client
{
    public class CreativeTonieUploadRequest
    {
        public class Entry
        {
            public Stream File { get; set; }
            public string Name { get; set; }
        }

        public IEnumerable<Entry> Entries { get; set; }
        public string CreativeTonieId { get; set; }
        public string HouseholdId { get; set; }
    }
}
