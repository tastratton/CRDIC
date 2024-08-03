using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.NamingConventionBinder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using static HostingPlayground.HostingPlaygroundLogEvents;
using System;
using Microsoft.Extensions.Configuration;

using System.IO;
using HostingPlayground.CompositionRoot;
using System.Collections.Immutable;

namespace HostingPlayground;

class Program
{
    static async Task<int> Main(string[] args)
    {
        // add config processing to main so we can get the config before the DI container actually exists...
        // that way, we can configure it from config

        // get DOTNET_ENVIRONMENT... environment variables and commandline only first
        var configBuilder = new ConfigurationBuilder();
        var configRoot = configBuilder
            .AddEnvironmentVariables() //"DOTNET_"
            .AddCommandLine(args)
            .Build();
        String EnvironmentName = configRoot["ENVIRONMENT"];
        if (String.IsNullOrEmpty(EnvironmentName))
        {
            EnvironmentName = "Production";
        };

        foreach ((string key, string value) in configRoot.AsEnumerable().ToImmutableSortedDictionary())
        {
            Console.WriteLine($"'{key}' = '{value}'");
        }

        configRoot = configBuilder
        .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: false)
        .AddJsonFile(path: $"appsettings.{EnvironmentName}.json", optional: true, reloadOnChange: false)
        //.AddEnvironmentVariables()  // << if need ALL the env vars!
        .AddEnvironmentVariables(prefix: "DOTNET_")  // just env vars starting with DOTNET_, strips the DOTNET_ from the result
        .AddCommandLine(args)
        .Build();

        foreach ((string key, string value) in configRoot.AsEnumerable().ToImmutableSortedDictionary())
        {
            Console.WriteLine($"'{key}' = '{value}'");
        }

        IHostBuilder hostBuilder = HostingPlayGroundCompositionRoot.GetHostBuilder(args);
        var commandLineParser = BuildCommandLine()
            .UseHost(args => hostBuilder, HostingPlayGroundCompositionRoot.ActionConfigureServices)
            .UseDefaults()
            .Build();
        return await commandLineParser.InvokeAsync(args);
    }

    private static CommandLineBuilder BuildCommandLine()
    {
        var root = new RootCommand(@"$ dotnet run --name 'Joe'"){
            new Option<string>("--name"){
                IsRequired = true
            }
        };
        root.Handler = CommandHandler.Create<OldGreeter.GreeterOptions, IHost>(Run);
        return new CommandLineBuilder(root);
    }

    private static void Run(OldGreeter.GreeterOptions options, IHost host)
    {
        var serviceProvider = host.Services;
        var greeter = serviceProvider.GetRequiredService<OldGreeter.IGreeter>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger(typeof(Program));

        var name = options.Name;
        logger.LogInformation(GreetEvent, "Greeting was requested for: {name}", name);
        greeter.Greet(name);
    }
}
