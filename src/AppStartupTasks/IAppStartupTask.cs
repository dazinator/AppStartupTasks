namespace AppStartupTasks;

public interface IAppStartupTask
{
    Task InitialiseAsync(CancellationToken ct);
}
