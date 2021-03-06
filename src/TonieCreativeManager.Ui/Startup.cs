using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TonieCreativeManager.Service;
using TonieCloud;
using TonieCreativeManager.Service.Model;
using TonieCreativeManager.Ui.Service;

namespace TonieCreativeManager.Ui
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var config = Configuration.GetSection("TonieCreativeManager").Get<Settings>();
            config.LibraryRoot = Configuration["MEDIA_LIBRARY"].TrimEnd('\\').TrimEnd('/');
            services.AddSingleton(config);
            services.AddSingleton(new Login { Email = Configuration["MYTONIE_LOGIN"], Password = Configuration["MYTONIE_PASSWORD"] });
            
            services.AddSingleton<TonieCloudClient>();
            services.AddSingleton<TonieCloudService>();
            services.AddSingleton<CreativeTonieService>();
            services.AddSingleton<MediaService>();
            services.AddSingleton<RepositoryService>();
            services.AddSingleton<UserService>();
            services.AddSingleton<VoucherService>();
            services.AddSingleton<SessionService>();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpContextAccessor();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
