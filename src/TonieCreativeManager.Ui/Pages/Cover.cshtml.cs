using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Net.Http.Headers;
using System.Threading.Tasks;
using TonieCreativeManager.Service;

namespace TonieCreativeManager.Ui.Pages
{
    [ResponseCache(Location = ResponseCacheLocation.Client, Duration = 3600)]
    public class Cover : PageModel
    {
        private readonly MediaService mediaService;

        public Cover(MediaService mediaService)
        {
            this.mediaService = mediaService;
        }

        public async Task<FileStreamResult> OnGetAsync()
        {
            var path = Request.Query["path"];

            var cover = await mediaService.GetCover(path);

            return new FileStreamResult(cover.Data, new MediaTypeHeaderValue(cover.MimeType));
        }
    }
}
