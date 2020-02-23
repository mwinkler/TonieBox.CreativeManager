using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using TonieBox.Service;

namespace TonieBox.Ui.Delegates
{
    public class CoverHandler
    {
        private readonly FileService fileService;

        public CoverHandler(FileService fileService)
        {
            this.fileService = fileService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Query["path"];

            var cover = await fileService.GetDirectoryCover(path);
            
            context.Response.ContentType = cover.MimeType;

            await cover.Data.CopyToAsync(context.Response.Body);
        }
    }
}
