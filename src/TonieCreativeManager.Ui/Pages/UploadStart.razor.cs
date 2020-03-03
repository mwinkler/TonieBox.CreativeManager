using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TonieCreativeManager.Service;
using TonieCreativeManager.Ui.Model;

namespace TonieCreativeManager.Ui.Pages
{
    public partial class UploadStart
    {
        [Inject] private TonieboxService TonieboxService { get; set; }

        [Inject] public IHttpContextAccessor HttpContext { get; set; }

        [Parameter] public string TonieId { get; set; }

        [Parameter] public string HouseholdId { get; set; }

        public string CoverUrl { get; set; }

        public string TonieUrl { get; set; }

        public string PostUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var path = HttpContext.HttpContext.Request.Query["path"].ToString();
            
            var tonie = await TonieboxService.GetCreativeTonie(HouseholdId, TonieId);

            CoverUrl = $"/cover?path={path.EncodeUrl()}";
            TonieUrl = tonie.ImageUrl;

            PostUrl = $"/upload/{HouseholdId}/{TonieId}?path={path.EncodeUrl()}";
        }
    }
}
