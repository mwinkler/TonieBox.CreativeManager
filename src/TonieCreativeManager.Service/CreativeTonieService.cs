﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TonieCloud;

namespace TonieCreativeManager.Service
{
    public class CreativeTonieService
    {
        private readonly Settings settings;
        private readonly TonieCloudService tonieCloudService;
        private readonly MappingService mappingService;
        private IEnumerable<Tonie> tonies;

        public CreativeTonieService(Settings settings, TonieCloudService tonieCloudService, MappingService mappingService)
        {
            this.settings = settings;
            this.tonieCloudService = tonieCloudService;
            this.mappingService = mappingService;
        }

        public async Task<IEnumerable<Tonie>> GetTonies()
        {
            if (tonies == null)
            {
                tonieCloudService.RefreshCreativeTonies();

                var cts = await tonieCloudService.GetCreativeTonies();

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

        public async Task<Tonie> GetTonie(string tonieId) => (await GetTonies()).FirstOrDefault(tonie => tonie.Id == tonieId) ?? throw new Exception($"Creative tonie with id '{tonieId}' was not found");

        public async Task<CreativeTonie> Upload(string path, string creativeTonieId)
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
                HouseholdId = (await tonieCloudService.GetHousehold()).Id,
                TonieName = Path.GetFileName(path),
                Entries = files
            };

            // upload media to tonie cloud
            var response = await tonieCloudService.UploadFilesToCreateiveTonie(request);

            // wait until transcoding is finished
            while (response.Transcoding)
            {
                await Task.Delay(TimeSpan.FromSeconds(10));

                response = await tonieCloudService.GetCreativeTonie(creativeTonieId);
            }

            // save mapping
            await mappingService.SetMapping(creativeTonieId, path);

            // reset creative tonies
            tonies = null;

            return response;
        }
    }
}