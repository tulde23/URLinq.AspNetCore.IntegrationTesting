using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.AspNetCore;
using URLinq.CLI.Contracts;

namespace URLinq.CLI
{
    internal class Program
    {
        //
        // private static async Task Main(string[] args)
        private static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .UseConsoleLifetime()
                .ConfigureAppConfiguration(
                    (hostingContext, config) => { }
                 )
                 .ConfigureServices((hostingContext, services) =>
                 {
                     services.AddSingleton<ISolutionAnalyzer, RoslynSolutionAnalyzer>()
                     .AddTransient<IIntegrationTestGenerator, TestGenerator>()
                                .AddSingleton<ILoggerFactory>(ConfigureSerilog);
                 })
                 .ConfigureLogging((hostingContext, logging) =>
                 {
                 });

            var path = @"C:\Development\aggregato\Aggregato.API.sln";
            var host = builder.Build();
            var service = host.Services.GetService<ISolutionAnalyzer>();
            var generator = host.Services.GetService<IIntegrationTestGenerator>();
           var projects = await service.Analyze(path);

           
            foreach( var p in projects.SelectMany(x=>  x.Controllers))
            {
                generator.Generate(p).GetAwaiter().GetResult();
                generator.WriteTo(@"C:\\temp\\" + p.ControllerName + "Tests.cs").GetAwaiter().GetResult();
            }
           var data = JsonConvert.SerializeObject(projects, Formatting.Indented);
            File.WriteAllText("Controllers.json", data);


        }
        private static SerilogLoggerFactory ConfigureSerilog(IServiceProvider s)
        {
            string outputFormat = "{SourceContext:l}[{Timestamp:HH:mm:ss} {Level}] {Message:lj}{NewLine}{Exception}";
            var loggerConfiguration = new LoggerConfiguration()
                  .Enrich.FromLogContext()
              .WriteTo.Console(outputTemplate: outputFormat);
            Serilog.ILogger logger = loggerConfiguration.MinimumLevel.Debug().CreateLogger();

            return new SerilogLoggerFactory(logger, true);
        }
    }
}