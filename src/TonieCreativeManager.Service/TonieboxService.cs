using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TonieCloud;

namespace TonieCreativeManager.Service
{
    public class TonieboxService
    {
        private readonly TonieCloudClient client;
        private readonly Settings settings;
        private readonly MappingService mappingService;
        private IEnumerable<Tonie> tonies;
        private IEnumerable<Household> households;

        public TonieboxService(TonieCloudClient client, Settings settings, MappingService mappingService)
        {
            this.client = client;
            this.settings = settings;
            this.mappingService = mappingService;
        }

        public async Task<IEnumerable<Household>> GetHouseholds() => households ?? (households = await client.GetHouseholds());

        public async Task<IEnumerable<Tonie>> GetCreativeTonies(string householdId)
        {
            if (tonies == null)
            {
                var cts = await client.GetCreativeTonies(householdId);

                var mappings = await mappingService.GetMappings();

                tonies = cts
                    .Select(t => new Tonie
                    {
                        Id = t.Id,
                        ImageUrl = t.ImageUrl,
                        Name = t.Name,
                        CurrentMediaPath = mappings.FirstOrDefault(m => m.TonieId == t.Id)?.Path
                    })
                    .ToArray();
            }

            return tonies;
        }
        
        public Task<CreativeTonie> GetCreativeTonie(string householdId, string creativeTonieId) => client.GetCreativeTonie(householdId, creativeTonieId);

        public async Task<CreativeTonie> Upload(string path, string householdId, string creativeTonieId)
        {
            var files = System.IO.Directory.GetFiles(settings.LibraryRoot + path)
                .Where(p => settings.SupportedFileExtensions.Contains(Path.GetExtension(p), StringComparer.OrdinalIgnoreCase))
                .OrderBy(p => p)
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

            // upload media to tonie cloud
            var response = await client.UploadFilesToCreateiveTonie(request);

            // wait until transcoding is finished
            while (response.Transcoding)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));

                response = await client.GetCreativeTonie(householdId, creativeTonieId);
            }

            // save mapping
            await mappingService.SetMapping(creativeTonieId, path);

            return response;
        }
    }
}
