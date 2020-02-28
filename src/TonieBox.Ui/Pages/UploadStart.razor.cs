using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TonieBox.Service;
using TonieBox.Ui.Model;

namespace TonieBox.Ui.Pages
{
    public partial class UploadStart
    {
        [Inject] private TonieboxService TonieboxService { get; set; }

        [Parameter] public string Path { get; set; }

        [Parameter] public string TonieId { get; set; }

        [Parameter] public string HouseholdId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var path = Path.DecodeUrl();
            
            var tonie = await TonieboxService.GetCreativeTonie(HouseholdId, TonieId);



        }
    }
}
