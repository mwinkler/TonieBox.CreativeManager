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
using TonieCreativeManager.Ui.Delegates;
using TonieCloud;

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
            var config = Configuration.GetSection("TonieBoxCreativeManager").Get<Settings>();
            config.LibraryRoot = Configuration["MEDIA_LIBRARY"].TrimEnd('\\').TrimEnd('/');
            services.AddSingleton(config);
            services.AddSingleton(new Login { Email = Configuration["MYTONIE_LOGIN"], Password = Configuration["MYTONIE_PASSWORD"] });
            
            services.AddSingleton<TonieCloudClient>();
            services.AddSingleton<TonieCloudService>();
            services.AddSingleton<CreativeTonieService>();
            services.AddSingleton<MediaService>();
            services.AddSingleton<MappingService>();
            services.AddSingleton<UserService>();
            services.AddScoped<CoverHandler>();
            services.AddScoped<UploadHandler>();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpContextAccessor();

            Console.WriteLine($"Proxy: {Configuration["HTTPS_PROXY"]}");
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
                endpoints.MapGet("/cover", ctx => ctx.RequestServices.GetRequiredService<CoverHandler>().InvokeAsync(ctx));
                endpoints.MapPost("/upload/{tonieId}", ctx => ctx.RequestServices.GetRequiredService<UploadHandler>().InvokeAsync(ctx));
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}