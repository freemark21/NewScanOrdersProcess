using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;

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
                .WriteTo.File(@"\\REP-APP\temp\NewScanOrdersProcess\log.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();


            Log.Information("Application Started.");


            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IFileWriteService, FileWriteService>();
                    services.AddTransient<ISendEmailService, SendEmailService>();
                })
                .UseSerilog()
                .Build();

            var svc = ActivatorUtilities.CreateInstance<FileWriteService>(host.Services);
            var svc2 = ActivatorUtilities.CreateInstance<SendEmailService>(host.Services);
            //svc.Run();
            svc2.Run();
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();
        }
    }
}
