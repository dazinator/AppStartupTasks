namespace AppStartupTasks;

public static class RegisterAppStartupTaskBuilderExtensions
{
    public static IRegisterAppStartupTaskBuilder<IAppStartupTask> AddAsyncTask(this IRegisterAppStartupTaskBuilder<IAppStartupTask> builder, Func<IServiceProvider, Task> initialisationScopeTasks)
    {
        builder.Add<AsyncDelegateAppStartupTask>(sp => new AsyncDelegateAppStartupTask(sp, initialisationScopeTasks));
        return builder;
    }
}
