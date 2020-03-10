using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TonieCreativeManager.Service.Model;

namespace TonieCreativeManager.Service
{
    public class UserService
    {
        private readonly CreativeTonieService creativeTonieService;
        private readonly RepositoryService repositoryService;
        private readonly VoucherService voucherService;
        private readonly Settings settings;
        private readonly MediaService mediaService;

        public UserService(CreativeTonieService creativeTonieService, RepositoryService repositoryService, VoucherService voucherService, Settings settings, MediaService mediaService)
        {
            this.creativeTonieService = creativeTonieService;
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

            return user.Credits >= settings.MediaItemBuyCost;
        }

        public async Task<bool> CanUploadItem(string userId)
        {
            var user = await GetUser(userId);

            return user.Credits >= settings.MediaItemUploadCost;
        }

        public async Task<PersistentData.Voucher> RedeemVoucher(string code, string userId)
        {
            var user = await GetUser(userId);

            // redeem voucher
            var voucher = await voucherService.Redeem(code);

            // credit user
            user.Credits += voucher.Value;

            // save user
            await repositoryService.SetUser(user);

            return voucher;
        }

        public async Task BuyItem(string userId, string path)
        {
            var user = await GetUser(userId);

            // check credit
            if (user.Credits < settings.MediaItemBuyCost)
            {
                throw new Exception("Insufficient credits");
            }

            // subtract credit
            user.Credits -= settings.MediaItemBuyCost;

            // reward back one credit to allow upload
            user.Credits++;

            // save user
            await repositoryService.SetUser(user);

            // mark path as bought
            await mediaService.MarkFolderAsBought(path);
        }

        public async Task UploadItem(string userId, string path, string creativeTonieId)
        {
            var user = await GetUser(userId);

            // check credit
            if (user.Credits < settings.MediaItemUploadCost)
            {
                throw new Exception("Insufficient credits");
            }

            // upload
            await creativeTonieService.Upload(path, creativeTonieId);

            // subtract credit
            user.Credits -= settings.MediaItemUploadCost;

            // save user
            await repositoryService.SetUser(user);
        }
    }
}
