namespace FileSystemWatcherService.Watchers;

public interface IFsWatcher
{
    void Watch(string directory, CancellationToken cancellationToken);
}