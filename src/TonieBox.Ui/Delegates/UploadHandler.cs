using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Threading.Tasks;
using TonieBox.Service;

namespace TonieBox.Ui.Delegates
{
    public class UploadHandler
    {
        private readonly TonieboxService tonieboxService;

        public UploadHandler(TonieboxService tonieboxService)
        {
            this.tonieboxService = tonieboxService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Query["path"];
            var householdId = (string)context.GetRouteValue("householdId");
            var tonieId = (string)context.GetRouteValue("tonieId");

            await tonieboxService.Upload(path, householdId, tonieId);
            //await Task.Delay(3000);

            context.Response.Redirect("/browse");
        }
    }
}
