using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TonieCreativeManager.Service;
using TonieCreativeManager.Ui.Model;
using TonieCreativeManager.Service;

namespace TonieCreativeManager.Ui.Pages
{
    public partial class SelectTonie
    {
        [Inject] private TonieboxService TonieboxService { get; set; }

        [Inject] public IHttpContextAccessor HttpContext { get; set; }

        private IEnumerable<Item> Tonies;

        private string BackUrl;

        protected override async Task OnInitializedAsync()
        {
            var path =  HttpContext.HttpContext.Request.Query["path"].ToString();

            BackUrl = $"/browse/{path.GetParentPath().EncodeUrl()}";

            var household = (await TonieboxService.GetHouseholds()).FirstOrDefault() ?? throw new Exception("No household found");
            var tonies = await TonieboxService.GetCreativeTonies(household.Id);

            Tonies = tonies
                .Select(t => new Item
                {
                    CoverUrl = t.ImageUrl,
                    Name = t.Name,
                    Url = $"/uploadstart/{household.Id}/{t.Id}?path={path.EncodeUrl()}",
                    SubCoverUrl = t.CurrentMediaPath != null ? $"/cover?path={t.CurrentMediaPath.EncodeUrl()}" : null
                })
                .ToArray();
        }
    }
}
