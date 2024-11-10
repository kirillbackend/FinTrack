using Autofac.Extensions.DependencyInjection;
using Serilog;

namespace FinTrack.RestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
              .UseServiceProviderFactory(new AutofacServiceProviderFactory())
              .ConfigureAppConfiguration((hostingContext, config) => {
                  var env = hostingContext.HostingEnvironment;
                  config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables("FinTrack_");
              })
          .ConfigureWebHostDefaults(webHostBuilder => {
              webHostBuilder
               .UseContentRoot(Directory.GetCurrentDirectory())
              .UseIISIntegration()
               .UseStartup<Startup>();
          })
           .UseSerilog((hostContext, services, configuration) => {
               configuration.WriteTo.Console();
           })
           .Build();

            host.Run();
        }
    }
}



