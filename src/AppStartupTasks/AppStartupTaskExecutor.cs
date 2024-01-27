namespace AppStartupTasks;

using Microsoft.Extensions.DependencyInjection;

public class AppStartupTaskExecutor : IAppStartupTaskExecutor
{
    private readonly IServiceProvider _sp;

    public AppStartupTaskExecutor(IServiceProvider sp) => _sp = sp;

    public async Task ExecuteAsync(CancellationToken ct)
    {
        var tasks = _sp.GetServices<IAppStartupTask>().ToArray();
        if (!tasks?.Any() ?? false)
        {
            return;
        }

        var asyncTasks = tasks!.Select(a => a.InitialiseAsync(ct)).ToArray();
        await Task.WhenAll(asyncTasks).WaitAsync(ct);
    }
}
