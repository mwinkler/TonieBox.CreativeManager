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
            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.AddSingleton(sp => Configuration.GetSection("TonieBoxCreativeManager").Get<Settings>());
            services.AddSingleton(new Login { Email = Configuration["MYTONIE_LOGIN"], Password = Configuration["MYTONIE_PASSWORD"] });
            services.AddSingleton<TonieboxClient>();
            services.AddSingleton<TonieboxService>();
            services.AddSingleton<FileService>();
            services.AddScoped<CoverHandler>();
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
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
