using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TonieBox.Client;

namespace TonieBox.Service
{
    public class TonieboxService
    {
        private readonly TonieboxClient client;
        private readonly Settings settings;

        public TonieboxService(TonieboxClient client, Settings settings)
        {
            this.client = client;
            this.settings = settings;
        }

        public Task<Household[]> GetHouseholds() => client.GetHouseholds();

        public Task<CreativeTonie[]> GetCreativeTonies(string householdId) => client.GetCreativeTonies(householdId);
        
        public Task<CreativeTonie> GetCreativeTonie(string householdId, string creativeTonieId) => client.GetCreativeTonie(householdId, creativeTonieId);

        public async Task Upload(string path, string householdId, string creativeTonieId)
        {
            var files = System.IO.Directory.GetFiles(settings.LibraryRoot + path)
                .Where(p => settings.SupportedFileExtensions.Contains(Path.GetExtension(p), StringComparer.OrdinalIgnoreCase))
                .Select(p => new UploadFilesToCreateiveTonieRequest.Entry
                {
                    File = File.OpenRead(p),
                    Name = Path.GetFileNameWithoutExtension(p)
                })
                .ToArray();

            var request = new UploadFilesToCreateiveTonieRequest
            {
                CreativeTonieId = creativeTonieId,
                HouseholdId = householdId,
                TonieName = Path.GetFileName(path),
                Entries = files
            };

            var response = await client.UploadFilesToCreateiveTonie(request);
        }
    }
}
