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
        
        [Inject] private TonieboxService TonieboxService { get; set; }

        [Inject] public IHttpContextAccessor HttpContext { get; set; }

        private IEnumerable<Item> Items;

        private string BackPath;

        protected override async Task OnInitializedAsync()
        {
            var path = HttpContext.HttpContext.Request.Query["path"].ToString();
            
            var directories = await MediaService.GetDirectories(path);
            var households = await TonieboxService.GetHouseholds();
            var tonies = await TonieboxService.GetCreativeTonies(households.First().Id);

            BackPath = string.IsNullOrEmpty(path)
                ? null
                : $"/browse?path={path.GetParentPath().EncodeUrl()}";

            Items = directories
                .Select(dir => new Item
                {
                    Name = dir.Name,
                    Url = $"/{(dir.HasSubfolders ? "browse" : "selecttonie")}?path={dir.Path.EncodeUrl()}",
                    CoverUrl = $"/cover?path={dir.Path.EncodeUrl()}",
                    SubCoverUrl = dir.HasSubfolders
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
