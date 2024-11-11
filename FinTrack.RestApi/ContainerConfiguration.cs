using Autofac;
using Serilog.Settings.Configuration;
using Serilog;
using Serilog.Formatting.Compact;

namespace FinTrack.RestApi
{
    public class ContainerConfiguration
    {
        public static void ResisterTypes(ContainerBuilder builder, ApiSettings settings)
        {
            builder.RegisterInstance(settings);

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("serilog.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                 .MinimumLevel.Information()
                 .WriteTo.Console(new CompactJsonFormatter())
                 .WriteTo.File($@"D:\data\FinTrack\Year-{DateTime.Now.Year}\Month-{DateTime.Now.Month}\Log-{DateTime.Now.Day}.txt")
                .ReadFrom.Configuration(configuration,
                    new ConfigurationReaderOptions() { SectionName = "Serilog" })
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.RegisterInstance(new LoggerFactory())
                .As<ILoggerFactory>();

            builder.RegisterGeneric(typeof(Logger<>))
                   .As(typeof(ILogger<>))
                   .SingleInstance();

            //builder.RegisterType<UserContextActionFilter>().AsSelf();

            Services.ContainerConfiguration.RegisterTypes(builder, settings);

            Log.Information("FinTrack.RestApi.ContainerConfiguration.RegisterTypes completed");
        }
    }
}
