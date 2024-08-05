using System;
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
        var config = ProgramConfig.GetConfig(args, configBuilder);
        
        //Console.WriteLine($"logger from config is {config["HostingPlayground:Logger"]}");

        IHostBuilder hostBuilder = HostingPlayGroundCompositionRoot.GetHostBuilder(args);
        Parser commandLineParser = Commands.GetCommandLineParser(hostBuilder, args);
        return await commandLineParser.InvokeAsync(args);
    }
 }
