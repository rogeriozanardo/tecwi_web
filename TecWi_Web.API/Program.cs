using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TecWi_Web.Data.Context;

namespace TecWi_Web.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHost iHost = CreateHostBuilder(args).Build();
            using (IServiceScope iServiceScope = iHost.Services.CreateScope())
            {
                IServiceProvider iServiceProvider = iServiceScope.ServiceProvider;
                ILoggerFactory iLoggerFactory = iServiceProvider.GetRequiredService<ILoggerFactory>();

                try
                {
                    DataContext dataContext = iServiceProvider.GetRequiredService<DataContext>();
                    await dataContext.Database.EnsureCreatedAsync();
                    await dataContext.Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    ILogger iLogger = iLoggerFactory.CreateLogger<Program>();
                    iLogger.LogError(ex, ex.Message);
                }
            }

            iHost.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
