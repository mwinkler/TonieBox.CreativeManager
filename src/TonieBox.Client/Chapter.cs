using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToniBox.Client
{
    public class Chapter
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string File { get; set; }
        public float Seconds { get; set; }
        public bool Transcoding { get; set; }
    }
}
