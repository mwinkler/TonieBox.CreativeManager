using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TonieCloud;
using System;

namespace TonieCreativeManager.Service
{
    public class TonieCloudService
    {
        private readonly TonieCloudClient client;

        public TonieCloudService(TonieCloudClient client)
        {
            this.client = client;
        }

        private IEnumerable<CreativeTonie> creativeTonies;
        private IEnumerable<Household> households;
        private IEnumerable<Toniebox> tonieboxes;

        public async Task<Household> GetHousehold() => (await GetHouseholds()).FirstOrDefault() ?? throw new Exception("No household found");

        public async Task<IEnumerable<Household>> GetHouseholds() => households ?? (households = await client.GetHouseholds());

        public async Task<IEnumerable<CreativeTonie>> GetCreativeTonies() => creativeTonies ?? (creativeTonies = await client.GetCreativeTonies((await GetHousehold()).Id));
        
        public async Task<CreativeTonie> GetCreativeTonie(string creativeTonieId) => (await GetCreativeTonies()).FirstOrDefault(t => t.Id == creativeTonieId);

        public void RefreshCreativeTonies() => creativeTonies = null;

        public Task<CreativeTonie> UploadFilesToCreateiveTonie(UploadFilesToCreateiveTonieRequest request) => client.UploadFilesToCreateiveTonie(request);

        public async Task<IEnumerable<Toniebox>> GetTonieboxes() => tonieboxes ?? (tonieboxes = await client.GetTonieboxes((await GetHousehold()).Id));

    }
}
