using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TonieCreativeManager.Service.Model;

namespace TonieCreativeManager.Service
{
    public class RepositoryService
    {
        private readonly Settings settings;

        private string PersistentDataFilePath => Path.Combine(settings.LibraryRoot, settings.PersistentDataFile);

        private PersistentData data;

        public RepositoryService(Settings settings)
        {
            this.settings = settings;
        }

        private async Task<PersistentData> GetData()
        {
            if (data == null)
            {
                data = File.Exists(PersistentDataFilePath)
                    ? JsonConvert.DeserializeObject<PersistentData>(await File.ReadAllTextAsync(PersistentDataFilePath))
                    : new PersistentData();
            }

            return data;
        }

        public async Task<IEnumerable<PersistentData.TonieMapping>> GetMappings() => (await GetData()).TonieMappings;
        
        public async Task<IEnumerable<PersistentData.User>> GetUsers() => (await GetData()).Users;

        public async Task<PersistentData.TonieMapping> SetMapping(string creativeTonieId, string path)
        {
            var data = await GetData();
            var mapping = data.TonieMappings.FirstOrDefault(m => m.TonieId == creativeTonieId);

            if (mapping == null)
            {
                mapping = new PersistentData.TonieMapping { TonieId = creativeTonieId };

                data.TonieMappings.Add(mapping);
            }

            mapping.Path = path;

            await PersistData();

            return mapping;
        }

        private async Task PersistData()
        {
            var json = JsonConvert.SerializeObject(await GetData(), Formatting.Indented);

            await File.WriteAllTextAsync(PersistentDataFilePath, json);
        }
    }
}
