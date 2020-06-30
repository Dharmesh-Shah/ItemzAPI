// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ItemzApp.API.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace ItemzApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {

            // EXPLANATION: first lets build configuration that can be then used for Logger.
            // We are doing it in the Main public static void main method because we want it to 
            // start logging right from the main method itself. Otherwise we could have leveraged
            // Dependency Injection of ASP.NET Core to create logger later on.

            var configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile(path:"appsettings.json", optional: false, reloadOnChange: true)
               .Build();


            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                Log.Information("Starting Up");


                var host = CreateHostBuilder(args).Build();

                //var logger = host.Services.GetRequiredService<ILogger<Program>>(); // Program in code 'GetRequiredService<ILogger<Program>>' indicates Logging Category.

                // migrate the database. Best Practice = in main, using service scope

                using (var scope = host.Services.CreateScope())
                {
                    try
                    {
                        var context = scope.ServiceProvider.GetService<ItemzContext>();
                        // for demo purposes, delete the database & migrate on startup so 
                        // we can start with a clean slate
                        context.Database.EnsureDeleted();
                        context.Database.Migrate();
                    }
                    catch (Exception ex)
                    {
                        ////var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>(); 
                        //logger.LogError(ex, "An error occurred while migrating the database.");
                        Log.Fatal(ex, "An error occurred while migrating the database.");
                    }
                }

                //logger.LogInformation("Host created.");

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
