
using System;
using System.Collections.Generic;
using System.Linq;

namespace TonieCreativeManager.Service.Model
{
    public class MediaItem
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public IEnumerable<string> MappedTonieIds { get; set; } = Enumerable.Empty<string>();
        public bool HasBought { get; set; }
        public IEnumerable<MediaItem> Childs { get; set; }
        public DateTime Added { get; set; }
    }
}
