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
        private readonly MediaService mediaService;

        public UserService(TonieCloudService tonieCloudService, RepositoryService repositoryService, VoucherService voucherService, Settings settings, MediaService mediaService)
        {
            this.tonieCloudService = tonieCloudService;
            this.repositoryService = repositoryService;
            this.voucherService = voucherService;
            this.settings = settings;
            this.mediaService = mediaService;
        }

        public Task<IEnumerable<PersistentData.User>> GetUsers() => repositoryService.GetUsers();

        public async Task<PersistentData.User> GetUser(string id) => (await GetUsers()).FirstOrDefault(u => u.Id == id);

        public async Task<bool> CanBuyItem(string userId)
        {
            var user = await GetUser(userId);

            return user.Credits >= settings.MediaItemCost;
        }

        public async Task RedeemVoucher(string code, string userId)
        {
            var user = await GetUser(userId);

            // redeem voucher
            var voucher = await voucherService.Redeem(code);

            // credit user
            user.Credits += voucher.Value;

            // save user
            await repositoryService.SetUser(user);
        }

        public async Task BuyItem(string userId, string path)
        {
            var user = await GetUser(userId);

            // check credit
            if (user.Credits < settings.MediaItemCost)
            {
                throw new Exception("Insufficient credits");
            }

            // grab credit
            user.Credits -= settings.MediaItemCost;

            // update user
            await repositoryService.SetUser(user);

            // mark path as bought
            await mediaService.MarkFolderAsBought(path);
        }
    }
}
