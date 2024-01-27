namespace AppStartupTasks;

public interface IRegisterAppStartupTaskBuilder
{
    RegisterAppStartupTaskBuilder Add(Func<IServiceProvider, Task> initialisationScopeTasks);

    RegisterAppStartupTaskBuilder Add<T>()
        where T : class, IAppStartupTask;

    RegisterAppStartupTaskBuilder Add<T>(Func<IServiceProvider, T> factory)
        where T : class, IAppStartupTask;
}
