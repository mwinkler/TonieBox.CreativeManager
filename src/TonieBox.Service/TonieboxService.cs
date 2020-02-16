using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TonieBox.Client;

namespace TonieBox.Service
{
    public class TonieboxService
    {
        private readonly TonieboxClient client;

        public TonieboxService(TonieboxClient client)
        {
            this.client = client;
        }
    }
}
