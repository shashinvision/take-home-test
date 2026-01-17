using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Fundo.Applications.WebApi.Infraestructure;
using Fundo.Applications.WebApi.Data;
using NLog;
using NLog.Web;

namespace Fundo.Applications.WebApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("appsettings.json").GetCurrentClassLogger();

            try
            {
                logger.Info("Iniciando aplicación Fundo");

                var host = CreateWebHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                    var appLogger = loggerFactory.CreateLogger("Fundo.Applications.WebApi.Program");

                    try
                    {
                        appLogger.LogInformation("Ensuring database is created");
                        var db = services.GetRequiredService<FundoDbContext>();
                        db.Database.EnsureCreated();
                        appLogger.LogInformation("Running database seed");
                        SeedData.Initialize(services);
                    }
                    catch (Exception ex)
                    {
                        appLogger.LogError(ex, "Error occurred while seeding database");
                    }
                }

                host.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                logger.Info("Application shutting down");
                LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog();
        }
    }
}
