using System;
using System.Collections.Generic;
using System.IO;
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

        public async Task Upload(string path, string householdId, string creativeTonieId)
        {
            var folderName = Path.GetDirectoryName(path);


            var request = new UploadFilesToCreateiveTonieRequest
            {
                CreativeTonieId = creativeTonieId,
                HouseholdId = householdId,
                TonieName = folderName,

            };
        }
    }
}
