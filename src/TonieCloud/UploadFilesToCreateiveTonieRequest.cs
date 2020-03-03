using System.Collections.Generic;
using System.IO;

namespace TonieCloud
{
    public class UploadFilesToCreateiveTonieRequest
    {
        public class Entry
        {
            public Stream File { get; set; }
            public string Name { get; set; }
            internal string FileId { get; set; }
        }

        public IEnumerable<Entry> Entries { get; set; }
        public string CreativeTonieId { get; set; }
        public string HouseholdId { get; set; }
        public string TonieName { get; set; }
    }
}
