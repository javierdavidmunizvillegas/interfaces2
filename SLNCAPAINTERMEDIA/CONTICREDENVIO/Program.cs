using CONTICREDENVIO.Servicios;
using DataBaseAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace CONTICREDENVIO
{
    class Program
    {
        public static IConfigurationRoot configuration;
        
        static int Main(string[] args)
        {
            try
            {
                // Start!
                // Logger.FileLogger("APIVTA018002", "Inicio Lectura  " );
                MainAsync(args).Wait();
                

                    return 0;
            }
            catch
            {
                return 1;
            }

        }
        static async Task MainAsync(string[] args)
        {
            // Create service collection
            //Log.Information("Creating service collection");

            ServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create service provider
            //Log.Information("Building service provider");
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            try
            {
                //Log.Information("Starting service");
                await serviceProvider.GetService<App>().Run();
                //Log.Information("Ending service");
            }
            catch (Exception ex)
            {
                //Log.Fatal(ex, "Error running service");
                throw ex;
            }
            finally
            {
                //Log.CloseAndFlush();
            }
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            
            // Build configuration
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);
            serviceCollection.AddTransient<App>();
            serviceCollection.AddTransient<IContDBConexionServicio, ContDBConexionServicio>();
            serviceCollection.AddTransient<IContColaServicio, ContColaServicio>();
            serviceCollection.AddTransient<IContLOG, ContLOG>();
        }
    }
}
