using Microsoft.AspNetCore.Http;

namespace TonieCreativeManager.Ui.Service
{
    public class SessionService
    {
        private readonly IHttpContextAccessor accessor;

        public SessionService(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
        }

        public string UserId
        {
            get => accessor.HttpContext.Request.Cookies["creativemanager_userid"];
            set => accessor.HttpContext.Response.Cookies.Append("creativemanager_userid", value);
        }
    }
}
