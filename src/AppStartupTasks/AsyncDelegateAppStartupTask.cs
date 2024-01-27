namespace AppStartupTasks;

public class AsyncDelegateAppStartupTask : IAppStartupTask
{
    private readonly Func<IServiceProvider, Task> _initialisationScopeTasks;
    private readonly IServiceProvider _sp;

    public AsyncDelegateAppStartupTask(IServiceProvider sp, Func<IServiceProvider, Task> initialisationScopeTasks)
    {
        _sp = sp;
        _initialisationScopeTasks = initialisationScopeTasks;
    }

    public Task InitialiseAsync(CancellationToken ct) => _initialisationScopeTasks(_sp);
}
