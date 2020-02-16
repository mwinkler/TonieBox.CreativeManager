using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TonieBox.Service
{
    public class Directory
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public byte[] Cover { get; set; }
        public string ParentPath { get; set; }
        public bool HasSubfolders { get; set; }
    }
}
