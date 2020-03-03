using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using TonieCreativeManager.Service;

namespace TonieCreativeManager.Ui.Delegates
{
    public class UploadHandler
    {
        private readonly CreativeTonieService creativeTonieService;

        public UploadHandler(CreativeTonieService creativeTonieService)
        {
            this.creativeTonieService = creativeTonieService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Query["path"];
            var tonieId = (string)context.GetRouteValue("tonieId");

            await creativeTonieService.Upload(path, tonieId);
            //await Task.Delay(3000);

            context.Response.Redirect("/browse");
        }
    }
}
