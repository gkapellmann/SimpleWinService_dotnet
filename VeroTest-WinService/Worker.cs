using VeronicaGen.Logger;

namespace TestService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHostApplicationLifetime lifetime;

    public Worker(ILogger<Worker> logger, IHostApplicationLifetime _lifetime)
    {

        _logger = logger;
        lifetime = _lifetime;

        lifetime.ApplicationStarted.Register(() => logger.LogInformation("ApplicationStarted Done..."));
        lifetime.ApplicationStopping.Register(() => logger.LogInformation("ApplicationStopping Done..."));
        lifetime.ApplicationStopped.Register(() => logger.LogInformation("ApplicationStopped Done..."));

        Log.Text("Constructor...");
    }

    public override Task StartAsync(CancellationToken cancellationToken) {
        _logger.LogInformation("Worker StartAsync at: {time}", DateTimeOffset.Now);
        Log.Text("StartAsync...");
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Log.Text("Start of ExecuteAsync...");
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(10000, stoppingToken);
        }
        Log.Text("End of ExecuteAsync...");
    }

    public override async Task<Task> StopAsync(CancellationToken cancellationToken) {
        _logger.LogInformation("Worker StopAsync at: {time}", DateTimeOffset.Now);
        Log.Text("StopAsync...");
        await Log.SaveCurrentLog();
        return base.StopAsync(cancellationToken);
    }

}
