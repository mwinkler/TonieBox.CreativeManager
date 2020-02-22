using System;
using System.Collections.Generic;
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

        public Task<Household[]> GetHouseholds() => client.GetHouseholds();

        public Task<CreativeTonie[]> GetCreativeTonies(string householdId) => client.GetCreativeTonies(householdId);
    }
}
