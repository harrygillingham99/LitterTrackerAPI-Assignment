using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace litter_tracker.API
{   /*
    The entry point for the application.
    */
    public class Program
    {
        public static void Main(string[] args)
        {
            //because I can't debug this locally with expo. Have to log absolutely everything when its deployed to my box
            var logging = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Staging"
                ? new LoggingLevelSwitch(LogEventLevel.Information)
                : new LoggingLevelSwitch(LogEventLevel.Error);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(logging)
                .Enrich.FromLogContext()
                .WriteTo.File(
                    new JsonFormatter(renderMessage: true),
                    Path.Combine(AppContext.BaseDirectory, "logs//Serilog.json"),
                    shared: true,
                    fileSizeLimitBytes: 20_971_520,
                    rollOnFileSizeLimit: true,
                    retainedFileCountLimit: 10)
                .CreateLogger();

            try
            {
                Log.Information("Starting Application");
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed to start");
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
