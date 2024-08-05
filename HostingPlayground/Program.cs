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
using HostingPlayground.CompositionRoot;
using Microsoft.Extensions.Configuration;


namespace HostingPlayground;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var configBuilder = new ConfigurationBuilder();
        ProgramConfig.GetConfig(args, configBuilder);
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
