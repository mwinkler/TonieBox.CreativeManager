using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TonieCreativeManager.Service;
using TonieCreativeManager.Ui.Model;

namespace TonieCreativeManager.Ui.Pages
{
    public partial class SelectUser
    {
        [Inject] public UserService UserService { get; set; }

        public IEnumerable<Item> Users { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var users = await UserService.GetUsers();

            Users = users
                .Select(u => new Item
                {
                    Name = u.Name,
                    CoverUrl = u.ProfileUrl
                })
                .ToArray();
        }
    }
}
