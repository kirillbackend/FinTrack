using Autofac;
using FinTrack.RestApi.ActionFilters;
using FinTrack.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Formatting.Compact;
using Serilog.Settings.Configuration;

namespace FinTrack.Tests
{
    public class ContainerConfiguration
    {
        public static void ResisterTypes(ContainerBuilder builder, FinTrackServiceSettings settings)
        {
            builder.RegisterInstance(settings);

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("serilog.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Information()
                 .WriteTo.Console(new CompactJsonFormatter())
                 .WriteTo.File($@"D:\data\FinTrack.Test\Year-{DateTime.Now.Year}\Month-{DateTime.Now.Month}\Log-{DateTime.Now.Day}.txt")
                .ReadFrom.Configuration(configuration,
                    new ConfigurationReaderOptions() { SectionName = "Serilog" })
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.RegisterInstance(new LoggerFactory())
                .As<ILoggerFactory>();

            builder.RegisterGeneric(typeof(Logger<>))
                   .As(typeof(ILogger<>))
                   .SingleInstance();

            Services.ContainerConfiguration.RegisterTypes(builder, settings);

            Log.Information("FinTrack.Tests.ContainerConfiguration.RegisterTypes completed");
        }
    }
}
