using System.CommandLine;
//using System.CommandLine.Builder;
//using System.CommandLine.Hosting;
//using System.CommandLine.NamingConventionBinder;
using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using static HostingPlayground.HostingPlaygroundLogEvents;
using HostingPlayground.CompositionRoot;
using Microsoft.Extensions.Configuration;
//using System.CommandLine.Help;
//using System.Linq;


namespace HostingPlayground;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var configBuilder = new ConfigurationBuilder();
        ProgramConfig.GetConfig(args, configBuilder);
        IHostBuilder hostBuilder = HostingPlayGroundCompositionRoot.GetHostBuilder(args);
        Parser commandLineParser = Commands.GetCommandLineParser(hostBuilder, args);
        return await commandLineParser.InvokeAsync(args);
    }

 /*
    private static CommandLineBuilder BuildCommandLine()
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
 */

    /*
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
    */
}
