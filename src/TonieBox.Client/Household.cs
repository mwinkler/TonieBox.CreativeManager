using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToniBox.Client
{
    public class Household
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool ForeignCreativeTonieContent { get; set; }
        public string Access { get; set; }
        public bool CanLeave { get; set; }
        public string OwnerName { get; set; }
    }
}
