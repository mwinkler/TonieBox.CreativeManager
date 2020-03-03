using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TonieCreativeManager.Service;

namespace TonieCreativeManager.Ui.Delegates
{
    public class CoverHandler
    {
        private readonly MediaService mediaService;

        public CoverHandler(MediaService mediaService)
        {
            this.mediaService = mediaService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Query["path"];

            var cover = await mediaService.GetCover(path);
            
            context.Response.ContentType = cover.MimeType;

            await cover.Data.CopyToAsync(context.Response.Body);
        }
    }
}
