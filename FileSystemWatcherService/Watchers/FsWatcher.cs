namespace FileSystemWatcherService.Watchers;

public class FsWatcher : IFsWatcher, IDisposable
{
    private readonly ILogger<FsWatcher> _logger;
    private FileSystemWatcher? _fsWatcher;

    public FsWatcher(ILogger<FsWatcher> logger)
    {
        _logger = logger;
    }

    public void Watch(string directory, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
            Unwatch();
        
        _fsWatcher = new FileSystemWatcher(directory);
        _fsWatcher.NotifyFilter = NotifyFilters.Attributes
                               | NotifyFilters.CreationTime
                               | NotifyFilters.DirectoryName
                               | NotifyFilters.FileName
                               | NotifyFilters.LastAccess
                               | NotifyFilters.LastWrite
                               | NotifyFilters.Security
                               | NotifyFilters.Size;

        _fsWatcher.Changed += OnChanged;
        _fsWatcher.Created += OnCreated;
        _fsWatcher.Deleted += OnDeleted;
        _fsWatcher.Renamed += OnRenamed;
        _fsWatcher.Error += OnError;

        _fsWatcher.Filter = "*.txt";
        _fsWatcher.IncludeSubdirectories = true;
        _fsWatcher.EnableRaisingEvents = true;

    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        if (e.ChangeType != WatcherChangeTypes.Changed)
        {
            return;
        }
        _logger.LogInformation($"Changed: {e.FullPath}");
    }

    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        var value = $"Created: {e.FullPath}";
        _logger.LogInformation(value);
    }

    private void OnDeleted(object sender, FileSystemEventArgs e)
    {
        _logger.LogInformation($"Deleted: {e.FullPath}");
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        _logger.LogInformation("Renamed:");
        _logger.LogInformation($"Old: {e.OldFullPath}");
        _logger.LogInformation($"New: {e.FullPath}");
    }

    private void OnError(object sender, ErrorEventArgs e)
    {
        var ex = e.GetException();
        _logger.LogError(ex, ex.Message);
    }

    private void Unwatch()
    {
        if(_fsWatcher is null) return;
        _fsWatcher.Changed -= OnChanged;
        _fsWatcher.Created -= OnCreated;
        _fsWatcher.Deleted -= OnDeleted;
        _fsWatcher.Renamed -= OnRenamed;
        _fsWatcher.Error -= OnError;
        _fsWatcher.Dispose();
        _fsWatcher = null;
    }

    public void Dispose()
    {
        Unwatch();
    }
}