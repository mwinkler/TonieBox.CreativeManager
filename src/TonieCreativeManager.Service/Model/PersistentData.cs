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

        public class UserData
        {
            public string Id { get; set; }
            public int Credits { get; set; }
        }

        public IList<TonieMapping> TonieMappings { get; set; } = new List<TonieMapping>();
        public IList<UserData> UserDatas { get; set; } = new List<UserData>();
    }
}
