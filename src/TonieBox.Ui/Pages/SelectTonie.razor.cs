using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TonieBox.Service;
using TonieBox.Ui.Model;

namespace TonieBox.Ui.Pages
{
    public partial class SelectTonie
    {
        [Inject] private TonieboxService TonieboxService { get; set; }

        [Parameter] public string Path { get; set; }

        private IEnumerable<Item> Tonies;

        private string BackUrl;

        protected override async Task OnInitializedAsync()
        {
            var path = Path.DecodeUrl();

            BackUrl = $"/browse/{path.GetParentPath().EncodeUrl()}";

            var household = (await TonieboxService.GetHouseholds()).FirstOrDefault() ?? throw new Exception("No household found");
            var tonies = await TonieboxService.GetCreativeTonies(household.Id);

            Tonies = tonies
                .Select(t => new Item
                {
                    CoverUrl = t.ImageUrl,
                    Name = t.Name,
                    Url = $"/uploadstart/{path.EncodeUrl()}/{household.Id}/{t.Id}"
                })
                .ToArray();
        }
    }
}
