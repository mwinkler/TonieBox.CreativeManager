using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TonieCreativeManager.Service;
using TonieCreativeManager.Ui.Model;

namespace TonieCreativeManager.Ui.Pages
{
    public partial class Browse
    {
        [Inject] private MediaService MediaService { get; set; }
        
        [Inject] private CreativeTonieService CreativeTonieService { get; set; }

        [Inject] public IHttpContextAccessor HttpContext { get; set; }

        private IEnumerable<Item> Items;

        private string BackPath;

        protected override async Task OnInitializedAsync()
        {
            var path = HttpContext.HttpContext.Request.Query["path"].ToString();
            
            var items = await MediaService.GetItems(path);
            var tonies = await CreativeTonieService.GetTonies();

            BackPath = string.IsNullOrEmpty(path)
                ? null
                : $"/browse?path={path.GetParentPath().EncodeUrl()}";

            Items = items
                .Select(dir => new Item
                {
                    Name = dir.Name,
                    Url = $"/{(dir.HasSubitems ? "browse" : "selecttonie")}?path={dir.Path.EncodeUrl()}",
                    CoverUrl = $"/cover?path={dir.Path.EncodeUrl()}",
                    SubCoverUrl = dir.HasSubitems
                        ? $"/cover?path=folder"
                        : dir.MappedTonieId != null
                            ? tonies.FirstOrDefault(t => dir.MappedTonieId == t.Id)?.ImageUrl
                            : null,
                    SubCoverClass = dir.MappedTonieId != null ? "tonie" : null
                })
                .ToArray();
        }
    }
}
