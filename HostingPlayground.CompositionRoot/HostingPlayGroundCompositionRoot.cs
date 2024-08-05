using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration; // TODO 1 - should we be using this directly here or passing args from program.cs?
using OldGreeter;

namespace HostingPlayground.CompositionRoot;
public static class HostingPlayGroundCompositionRoot
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IGreeter, Greeter>();
        return services;
    }

    public static Func<string[], IHostBuilder> GetHostBuilder = delegate (string[] args)
    {
        return Host.CreateDefaultBuilder(args)
        .UseDefaultServiceProvider((context, options) =>
        {
            options.ValidateScopes = true;
        })
        .ConfigureHostConfiguration((hostConfig) =>
        {
            // see TODO 1
            hostConfig.SetBasePath(Directory.GetCurrentDirectory());
            hostConfig.AddJsonFile("appsettings.json", optional: true);
            // end see TODO 1
            //hostConfig.AddEnvironmentVariables(prefix: ""); // prefix: "PREFIX_"); // DOTNET_ by default already added
            ;
            IConfigurationRoot config = hostConfig.Build();
            if (config["HostingPlayground:Logger"] == "EventLog") { Console.WriteLine ("detected eventlog logger in CompositionRoot GetHostBuilder hostConfig"); }
            

        })
        .ConfigureAppConfiguration((hostContext, appConfig) =>
        {
            IHostEnvironment env = hostContext.HostingEnvironment;
            Console.WriteLine($"isDevelopment: {env.IsDevelopment()}");
            // see TODO 1
            appConfig.AddCommandLine(args);
            // end see TODO 1
            IConfigurationRoot config = appConfig.Build();
            if (config["HostingPlayground:Logger"] == "EventLog") { Console.WriteLine("detected eventlog logger in CompositionRoot GetHostBuilder appConfig "); }

        })
        ;

    };

    /* -- not used?
    public static Action<IHostBuilder> ActionConfigureAppConfiguration = delegate (IHostBuilder builder)
    {
        builder.ConfigureServices((hostContext, services) =>
        {
            Console.WriteLine("ActionConfigureAppConfiguration");
            IConfiguration configuration = hostContext.Configuration;
            services.AddSingleton<IGreeter, Greeter>();
        });
    };
    */


    public static Action<IHostBuilder> ActionConfigureServices = delegate (IHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
            Console.WriteLine("ActionConfigureServices");
            // moved to compositionroot
            //services.AddSingleton<IGreeter, Greeter>();
            services.AddServices();
        });
    };



}
