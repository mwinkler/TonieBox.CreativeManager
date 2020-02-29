using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TonieBox.Service
{
    public class MappingService
    {
        private readonly Settings settings;

        private IList<TonieMapping> mappings;

        private string MappingFilePath => Path.Combine(settings.LibraryRoot, settings.MappingFile);

        public MappingService(Settings settings)
        {
            this.settings = settings;
        }

        public Task<IList<TonieMapping>> GetMappings()
        {
            if (mappings == null)
            {
                mappings = File.Exists(MappingFilePath)
                    ? JsonConvert.DeserializeObject<IList<TonieMapping>>(File.ReadAllText(MappingFilePath))
                    : new List<TonieMapping>();
            }

            return Task.FromResult(mappings);
        }

        public async Task<TonieMapping> SetMapping(string creativeTonieId, string path)
        {
            var mappings = await GetMappings();

            var mapping = mappings.FirstOrDefault(m => m.TonieId == creativeTonieId);

            if (mapping == null)
            {
                mapping = new TonieMapping { TonieId = creativeTonieId };
            }

            mapping.Path = path;

            await SaveMappings(mappings);

            return mapping;
        }

        private Task SaveMappings(IEnumerable<TonieMapping> mapping)
        {
            var json = JsonConvert.SerializeObject(mapping);

            File.WriteAllText(MappingFilePath, json);

            return Task.CompletedTask;
        }
    }
}
