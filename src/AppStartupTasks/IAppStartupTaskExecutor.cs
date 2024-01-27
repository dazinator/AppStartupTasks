namespace AppStartupTasks;

public interface IAppStartupTaskExecutor
{
    Task ExecuteAsync(CancellationToken ct);
}
