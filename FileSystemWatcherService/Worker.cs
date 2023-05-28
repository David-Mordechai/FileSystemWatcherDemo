using FileSystemWatcherService.Watchers;

namespace FileSystemWatcherService;

public class Worker : BackgroundService
{
    private readonly IFsWatcher _fsWatcher;

    public Worker(IFsWatcher fsWatcher)
    {
        _fsWatcher = fsWatcher;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _fsWatcher.Watch("C:\\Temp\\watch1", stoppingToken);
        return Task.CompletedTask;
    }
}