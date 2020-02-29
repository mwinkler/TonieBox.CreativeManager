using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TonieBox.Client;
using TonieBox.Service;
using TonieBox.Ui.Delegates;

namespace TonieBox.Ui
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
            config.LibraryRoot = Configuration["MEDIA_LIBRARY"];
            services.AddSingleton(config);
            services.AddSingleton(new Login { Email = Configuration["MYTONIE_LOGIN"], Password = Configuration["MYTONIE_PASSWORD"] });
            
            services.AddSingleton<TonieboxClient>();
            services.AddSingleton<TonieboxService>();
            services.AddSingleton<FileService>();
            services.AddSingleton<MappingService>();
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
                endpoints.MapPost("/upload/{householdId}/{tonieId}", ctx => ctx.RequestServices.GetRequiredService<UploadHandler>().InvokeAsync(ctx));
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
