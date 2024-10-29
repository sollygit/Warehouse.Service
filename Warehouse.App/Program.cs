using EasyConsoleCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Warehouse.Services;

namespace Warehouse.App
{
    internal class Program
    {
        static bool exit = false;
        static Menu menu;

        public static IConfiguration Configuration { get; private set; }
        public static ServiceProvider ServiceProvider { get; private set; }

        static IConfigurationBuilder Configure(IConfigurationBuilder config, string environmentName)
        {
            return config
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }

        static IConfiguration CreateConfiguration()
        {
            var env = new HostingEnvironment
            {
                EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production",
                ApplicationName = AppDomain.CurrentDomain.FriendlyName,
                ContentRootPath = AppDomain.CurrentDomain.BaseDirectory,
                ContentRootFileProvider = new PhysicalFileProvider(AppDomain.CurrentDomain.BaseDirectory)
            };

            var config = new ConfigurationBuilder();
            var configured = Configure(config, env.EnvironmentName);
            return configured.Build();
        }

        static void Run()
        {
            menu = new Menu()
                .Add("Products", new Action(async () => await Warehouse.Products()))
                .Add("Retailer Products", new Action(async () => await Warehouse.RetailerProducts()))
                .Add("Output Products", new Action(async () => await Warehouse.OutputProducts()))
                .Add("Exit", () => { exit = true; });

            while (!exit)
            {
                menu.Display();
                if (!exit)
                {
                    Console.WriteLine("Hit Enter to continue...");
                    Console.ReadLine();
                }
            }
        }

        static void Main(string[] _args)
        {
            Configuration = CreateConfiguration();

            // Configure Services
            var services = new ServiceCollection()
                .AddLogging(configure => configure.AddConsole())
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug)
                .AddSingleton(Configuration)
                .AddSingleton<IWarehouseService, WarehouseService>();

            ServiceProvider = services.BuildServiceProvider();
            Run();
        }
    }
}
