using HostingPlayground.CompositionRoot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Hosting;
using System.CommandLine.NamingConventionBinder;
using static HostingPlayground.HostingPlaygroundLogEvents;
using System.Linq;

namespace HostingPlayground;
internal class Commands
{
    public static CommandLineBuilder BuildCommandLine()
    {
        var root = new RootCommand(@"return a greeting"){
            new Option<string>(aliases: (["-n", "--name"]), description: "The name to add to the greeting" ){
                IsRequired = true
            }
        };
        root.Handler = CommandHandler.Create<OldGreeter.GreeterOptions, IHost>(Run);
        var sub1Command = new Command("dotest", "an example subcommand named dotest");
        root.AddCommand(sub1Command);

        return new CommandLineBuilder(root);
    }
    public static System.CommandLine.Parsing.Parser GetCommandLineParser(IHostBuilder hostBuilder, string[] args)
    {
        System.CommandLine.Parsing.Parser parser = Commands.BuildCommandLine()
            .UseHost(_ => hostBuilder, HostingPlayGroundCompositionRoot.ActionConfigureServices)
            .UseDefaults()
            .UseHelp(ctx =>
            {
                ctx.HelpBuilder.CustomizeLayout(_ =>
                    HelpBuilder.Default
                    .GetLayout()
                    .Append(_ => _.Output.WriteLine(""))
                    .Append(_ => _.Output.WriteLine("Example:"))
                    .Append(_ => _.Output.WriteLine("  $ HostingPlayground --name 'Joe'"))
                    );
            })
            .Build();
        return parser;
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
