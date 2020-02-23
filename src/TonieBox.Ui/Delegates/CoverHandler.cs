using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            context.Response.ContentType = "image/png";

            var img = await fileService.GetDirectoryCover(context.Request.Query["path"]);

            context.Response.BodyWriter.WriteAsync(img)
        }
    }
}
