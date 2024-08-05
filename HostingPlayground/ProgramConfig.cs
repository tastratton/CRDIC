using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Immutable;

namespace HostingPlayground;

/// <summary>
/// Get Configuration for Program.cs needed before Dependency Injection Container.
/// e.g. options that may be used to alter DI container setup later.
/// </summary>
internal class ProgramConfig
{

    // get DOTNET_ENVIRONMENT... environment variables and commandline only first
    public static IConfigurationRoot GetConfig(string[] args, ConfigurationBuilder configBuilder)
    {
        var configRoot = configBuilder
            .AddEnvironmentVariables() //"DOTNET_"
            .AddCommandLine(args)
            .Build();
        String EnvironmentName = configRoot["ENVIRONMENT"];
        if (String.IsNullOrEmpty(EnvironmentName))
        {
            EnvironmentName = "Production";
        };
        /* debug
        foreach ((string key, string value) in configRoot.AsEnumerable().ToImmutableSortedDictionary())
        {
            Console.WriteLine($"'{key}' = '{value}'");
        }
        */

        configRoot = configBuilder
        .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: false)
        .AddJsonFile(path: $"appsettings.{EnvironmentName}.json", optional: true, reloadOnChange: false)
        //.AddEnvironmentVariables()  // << if need ALL the env vars!
        .AddEnvironmentVariables(prefix: "DOTNET_")  // just env vars starting with DOTNET_, strips the DOTNET_ from the result
        .AddCommandLine(args)
        .Build();
        /* debug
        foreach ((string key, string value) in configRoot.AsEnumerable().ToImmutableSortedDictionary())
        {
            Console.WriteLine($"'{key}' = '{value}'");
        }
        */
        return configRoot;
    }

}