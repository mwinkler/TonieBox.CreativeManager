using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TonieCreativeManager.Service.Model;

namespace TonieCreativeManager.Service
{
    public class UserService
    {
        private readonly TonieCloudService tonieCloudService;
        private readonly RepositoryService repositoryService;
        private readonly VoucherService voucherService;
        private readonly Settings settings;
        private IEnumerable<User> users;

        public UserService(TonieCloudService tonieCloudService, RepositoryService repositoryService, VoucherService voucherService, Settings settings)
        {
            this.tonieCloudService = tonieCloudService;
            this.repositoryService = repositoryService;
            this.voucherService = voucherService;
            this.settings = settings;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            if (users == null)
            {
                var boxes = await tonieCloudService.GetTonieboxes();
                var data = await repositoryService.GetUsers();

                users = boxes
                    .Select(box => new User
                    {
                        Id = box.Id,
                        Name = box.Name,
                        ProfileUrl = box.ImageUrl,
                        Credits = data.FirstOrDefault(u => u.Id == box.Id)?.Credits ?? 0
                    })
                    .ToArray();
            }

            return users;
        }

        public async Task<User> GetUser(string id) => (await GetUsers()).FirstOrDefault(u => u.Id == id);

        public async Task<bool> CanBuyItem(string userId)
        {
            var user = await GetUser(userId);

            return user.Credits >= settings.MediaItemCost;
        }

        public async Task RedeemVoucher(string code, string userId)
        {
            var users = await repositoryService.GetUsers();
            var user = users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                user = new PersistentData.User { Id = userId };
            }

            // redeem voucher
            var voucher = await voucherService.Redeem(code);

            // credit user
            user.Credits += voucher.Value;

            // save user
            await repositoryService.SetUser(user);
        }
    }
}
