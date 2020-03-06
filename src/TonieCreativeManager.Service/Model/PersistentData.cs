using System;
using System.Collections.Generic;

namespace TonieCreativeManager.Service.Model
{
    public class PersistentData
    {
        public class TonieMapping
        {
            public string TonieId { get; set; }
            public string Path { get; set; }
        }

        public class User
        {
            public string Id { get; set; }
            public int Credits { get; set; }
        }

        public class Voucher
        {
            public string Code { get; set; }
            public int Value { get; set; }
            public DateTime? Used { get; set; }
        }

        public class BoughtItem
        {
            public string Path { get; set; }
            public int UserId { get; set; }
        }

        public IList<TonieMapping> TonieMappings { get; set; } = new List<TonieMapping>();
        public IList<User> Users { get; set; } = new List<User>();
        public IList<Voucher> Vouchers { get; set; } = new List<Voucher>();
        public IList<BoughtItem> BoughtItems { get; set; } = new List<BoughtItem>();
    }
}
