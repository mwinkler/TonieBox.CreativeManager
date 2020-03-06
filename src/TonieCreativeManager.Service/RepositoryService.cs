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

        public Task<IEnumerable<PersistentData.TonieMapping>> GetMappings() => GetData(d => d.TonieMappings);
        
        public Task<IEnumerable<PersistentData.User>> GetUsers() => GetData(d => d.Users);

        public Task<IEnumerable<PersistentData.Voucher>> GetVouchers() => GetData(d => d.Vouchers);

        public Task<IEnumerable<PersistentData.BoughtItem>> GetBoughtItems() => GetData(d => d.BoughtItems);

        public Task<PersistentData.TonieMapping> SetMapping(PersistentData.TonieMapping tonieMapping) => 
            SetValue(
                data => data.TonieMappings,
                mapping => mapping.TonieId == tonieMapping.TonieId,
                mapping =>
                {
                    mapping.TonieId = tonieMapping.TonieId;
                    mapping.Path = tonieMapping.Path;
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

        public Task<PersistentData.User> SetUser(PersistentData.User user) =>
            SetValue(
                data => data.Users,
                v => v.Id == user.Id,
                v =>
                {
                    v.Id = user.Id;
                    v.Credits = user.Credits;
                }
            );

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

        private async Task<IEnumerable<T>> GetData<T>(Func<PersistentData, IEnumerable<T>> select) => select.Invoke(await GetData());

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
