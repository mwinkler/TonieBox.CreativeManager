using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TonieCreativeManager.Service;

namespace TonieCreativeManager.Ui.Pages
{
    public partial class UploadStart
    {
        [Inject] private CreativeTonieService CreativeTonieService { get; set; }

        [Inject] public IHttpContextAccessor HttpContext { get; set; }

        [Parameter] public string TonieId { get; set; }

        public string CoverUrl { get; set; }

        public string TonieUrl { get; set; }

        public string PostUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var path = HttpContext.HttpContext.Request.Query["path"].ToString();
            
            var tonie = await CreativeTonieService.GetTonie(TonieId);

            CoverUrl = $"/cover?path={path.EncodeUrl()}";
            TonieUrl = tonie.ImageUrl;

            PostUrl = $"/upload/{TonieId}?path={path.EncodeUrl()}";
        }
    }
}
