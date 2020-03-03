using Newtonsoft.Json;
using System;
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

        public async Task<IEnumerable<PersistentData.Voucher>> GetVouchers() => (await GetData()).Vouchers;

        public Task<PersistentData.TonieMapping> SetMapping(string creativeTonieId, string path) => 
            SetValue(
                data => data.TonieMappings,
                mapping => mapping.TonieId == creativeTonieId,
                mapping =>
                {
                    mapping.TonieId = creativeTonieId;
                    mapping.Path = path;
                }
            );

        public Task<PersistentData.Voucher> SetVoucher(PersistentData.Voucher voucher) =>
            SetValue(
                data => data.Vouchers,
                v => v.Code == voucher.Code,
                v =>
                {
                    v.Code = voucher.Code;
                    v.Used = voucher.Used;
                    v.Value = voucher.Value;
                }
            );

        private async Task<T> SetValue<T>(Func<PersistentData, IList<T>> set, Func<T, bool> select, Action<T> update) where T : class
        {
            var data = await GetData();
            var list = set.Invoke(data);
            var value = list.FirstOrDefault(select);

            if (value == null)
            {
                value = Activator.CreateInstance<T>();

                list.Add(value);
            }

            update.Invoke(value);

            await PersistData();

            return value;
        }

        private async Task PersistData()
        {
            var json = JsonConvert.SerializeObject(await GetData(), Formatting.Indented);

            await File.WriteAllTextAsync(PersistentDataFilePath, json);
        }
    }
}
