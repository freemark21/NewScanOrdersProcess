using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Serilog;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace NewScanOrdersProcess
{
    class Program
    {
        
        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder();
            BuildConfig(builder);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.File(@"C:\Test\log.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();


            Log.Information("Application Started.");


            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IFileWriteService, FileWriteService>();
                })
                .UseSerilog()
                .Build();

            var svc = ActivatorUtilities.CreateInstance<IFileWriteService>(host.Services);
            svc.Run();
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();
        }
    }
}
