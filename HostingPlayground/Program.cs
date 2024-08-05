using System.CommandLine;
using Microsoft.Extensions.Hosting;
using System.CommandLine.Parsing;
using System.Threading.Tasks;
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
        Parser commandLineParser = Commands.GetCommandLineParser(hostBuilder, args);
        return await commandLineParser.InvokeAsync(args);
    }
 }
