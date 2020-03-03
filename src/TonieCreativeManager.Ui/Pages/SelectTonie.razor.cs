using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TonieCreativeManager.Service;
using TonieCreativeManager.Ui.Model;

namespace TonieCreativeManager.Ui.Pages
{
    public partial class SelectTonie
    {
        [Inject] private CreativeTonieService CreativeTonieService { get; set; }

        [Inject] public IHttpContextAccessor HttpContext { get; set; }

        private IEnumerable<Item> Tonies;

        private string BackUrl;

        protected override async Task OnInitializedAsync()
        {
            var path =  HttpContext.HttpContext.Request.Query["path"].ToString();

            BackUrl = $"/browse/{path.GetParentPath().EncodeUrl()}";

            var tonies = await CreativeTonieService.GetTonies();

            Tonies = tonies
                .Select(t => new Item
                {
                    CoverUrl = t.ImageUrl,
                    Name = t.Name,
                    Url = $"/uploadstart/{t.Id}?path={path.EncodeUrl()}",
                    SubCoverUrl = t.CurrentMediaPath != null ? $"/cover?path={t.CurrentMediaPath.EncodeUrl()}" : null
                })
                .ToArray();
        }
    }
}
