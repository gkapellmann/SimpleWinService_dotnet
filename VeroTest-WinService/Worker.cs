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
        int count = 0;
        Log.Text("Start of ExecuteAsync...");
        _logger.LogInformation("Start of ExecuteAsync: {time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            Log.Text("Delay...");
            await Task.Delay(10, stoppingToken);
            if(count > 500) {
                count = 0;
                Log.Text("Working ExecuteAsync...");
                _logger.LogInformation("Working ExecuteAsync: {time}", DateTimeOffset.Now);
            }
            count++;
        }

        _logger.LogInformation("End of ExecuteAsync: {time}", DateTimeOffset.Now);
        Log.Text("End of ExecuteAsync...");
        await Log.SaveCurrentLog();

    }

    public override async Task<Task> StopAsync(CancellationToken cancellationToken) {
        _logger.LogInformation("Worker StopAsync at: {time}", DateTimeOffset.Now);
        Log.Text("StopAsync...");
        await Log.SaveCurrentLog();
        return base.StopAsync(cancellationToken);
    }

}
