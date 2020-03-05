using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TonieCreativeManager.Service.Model;

namespace TonieCreativeManager.Service
{
    public class VoucherService
    {
        private readonly RepositoryService repositoryService;

        public VoucherService(RepositoryService repositoryService)
        {
            this.repositoryService = repositoryService;
        }

        public async Task<bool> IsValid(string code)
        {
            var vouchers = await repositoryService.GetVouchers();

            return vouchers.Any(v => v.Code == code && v.Used == null);
        }

        public async Task<PersistentData.Voucher> Redeem(string code)
        {
            var vouchers = await repositoryService.GetVouchers();
            var voucher = vouchers.FirstOrDefault(v => v.Code == code && v.Used == null);

            if (voucher == null)
            {
                throw new Exception($"Unable to redeem voucher '{code}'");
            }

            voucher.Used = DateTime.Now;

            await repositoryService.SetVoucher(voucher);

            return voucher;
        }
    }
}
