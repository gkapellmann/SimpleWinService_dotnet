using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;
using System.Runtime.InteropServices;
using TestService;
using VeronicaGen.Logger;

#pragma warning disable CA1416 // Validate platform compatibility

using var registration = PosixSignalRegistration.Create(PosixSignal.SIGTERM, (context) => context.Cancel = true);

LoggerConfigs.showLog = true;
LoggerConfigs.showAdvLog = true;
LoggerConfigs.saveLog = true;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options => {
        options.ServiceName = "Test Service";
    })
    .ConfigureServices(services => {

        LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(services);

        services.AddHostedService<Worker>();
    })
    .ConfigureLogging((context, logging) => {
        // See: https://github.com/dotnet/runtime/issues/47303
        logging.AddConfiguration(
            context.Configuration.GetSection("Logging"));
    })
    .Build();

host.Run();
