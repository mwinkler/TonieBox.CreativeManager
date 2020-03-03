using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TonieCreativeManager.Service.Model;

namespace TonieCreativeManager.Service
{
    public class UserService
    {
        private readonly TonieCloudService tonieCloudService;
        private IEnumerable<User> users;

        public UserService(TonieCloudService tonieCloudService)
        {
            this.tonieCloudService = tonieCloudService;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            if (users == null)
            {
                var boxes = await tonieCloudService.GetTonieboxes();

                users = boxes
                    .Select(box => new User
                    {
                        Id = box.Id,
                        Name = box.Name,
                        ProfileUrl = box.ImageUrl
                    })
                    .ToArray();
            }

            return users;
        }

    }
}
