using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Fundo.Applications.WebApi.Infraestructure;
using Fundo.Applications.WebApi.Data;

namespace Fundo.Applications.WebApi
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var host = CreateWebHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                    var logger = loggerFactory.CreateLogger("Fundo.Applications.WebApi.Program");


                    try
                    {
                        logger.LogInformation("Ensuring database is created");

                        var db = services.GetRequiredService<FundoDbContext>();
                        db.Database.EnsureCreated();

                        logger.LogInformation("Running database seed");
                        SeedData.Initialize(services);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Error occurred while seeding database");
                    }
                }

                host.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unhandled WebApi exception: {ex.Message}");
                throw;
            }
            finally
            {
                Console.WriteLine("Application shutting down.");
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        }
    }
}
