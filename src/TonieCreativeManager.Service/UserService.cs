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
        private IEnumerable<User> users;

        public UserService(TonieCloudService tonieCloudService, RepositoryService repositoryService)
        {
            this.tonieCloudService = tonieCloudService;
            this.repositoryService = repositoryService;
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

    }
}
