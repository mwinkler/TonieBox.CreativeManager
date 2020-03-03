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

        private PersistentData Data
        {
            get
            {
                if (data == null)
                {
                    data = File.Exists(PersistentDataFilePath)
                        ? JsonConvert.DeserializeObject<PersistentData>(File.ReadAllText(PersistentDataFilePath))
                        : new PersistentData();
                }

                return data;
            }
        }

        public IEnumerable<PersistentData.TonieMapping> GetMappings() => Data.TonieMappings;

        public async Task<PersistentData.TonieMapping> SetMapping(string creativeTonieId, string path)
        {
            var mapping = Data.TonieMappings.FirstOrDefault(m => m.TonieId == creativeTonieId);

            if (mapping == null)
            {
                mapping = new PersistentData.TonieMapping { TonieId = creativeTonieId };

                Data.TonieMappings.Add(mapping);
            }

            mapping.Path = path;

            await PersistData();

            return mapping;
        }

        private Task PersistData()
        {
            var json = JsonConvert.SerializeObject(Data, Formatting.Indented);

            File.WriteAllText(PersistentDataFilePath, json);

            return Task.CompletedTask;
        }
    }
}
