using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Syncfusion.Blazor;
using Syncfusion.Licensing;
using TecWi_Web.FrontServices;
using TecWi_Web.FrontServices.Interfaces;

namespace TecWi_Web.WASM
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            SyncfusionLicenseProvider.RegisterLicense("NTAxNzA5QDMxMzkyZTMyMmUzMFE0M2QrUm1JbjZEbjBSaFRHbmx3UjF5bEtFSHd0Rkx2YWV5YjNBU2F4aXM9");

            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            
            builder.Services.AddSyncfusionBlazor();

            builder.Services.AddSingleton<IUsuarioFrontService, UsuarioFrontService>();

            await builder.Build().RunAsync();
        }
    }
}
