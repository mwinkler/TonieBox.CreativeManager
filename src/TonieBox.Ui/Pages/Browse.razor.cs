using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TonieBox.Service;
using TonieBox.Ui.Model;

namespace TonieBox.Ui.Pages
{
    public partial class Browse
    {
        [Inject] private FileService FileService { get; set; }

        [Parameter] public string Path { get; set; }

        private IEnumerable<Item> Items;

        protected override async Task OnInitializedAsync()
        {
            var directories = await FileService.GetDirectory(HttpUtility.UrlDecode(Path ?? ""));

            Items = directories
                .Select(dir => new Item
                {
                    Name = dir.Name,
                    Url = $"/{(dir.HasSubfolders ? "browse" : "upload")}/{HttpUtility.UrlEncode(dir.Path)}",
                    CoverUrl = $"/cover?path={HttpUtility.UrlEncode(dir.Path)}"
                })
                .ToArray();
        }
    }
}
