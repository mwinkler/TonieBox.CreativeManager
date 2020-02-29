using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TonieBox.Service;
using TonieBox.Ui.Model;

namespace TonieBox.Ui.Pages
{
    public partial class Browse
    {
        [Inject] private FileService FileService { get; set; }

        [Inject] public IHttpContextAccessor HttpContext { get; set; }

        private IEnumerable<Item> Items;

        private string BackPath;

        protected override async Task OnInitializedAsync()
        {
            var path = HttpContext.HttpContext.Request.Query["path"].ToString();
            
            var directories = await FileService.GetDirectories(path);

            BackPath = string.IsNullOrEmpty(path)
                ? null
                : path.GetParentPath().EncodeUrl();

            Items = directories
                .Select(dir => new Item
                {
                    Name = dir.Name,
                    Url = $"/{(dir.HasSubfolders ? "browse" : "selecttonie")}?path={dir.Path.EncodeUrl()}",
                    CoverUrl = $"/cover?path={dir.Path.EncodeUrl()}"
                })
                .ToArray();
        }
    }
}
