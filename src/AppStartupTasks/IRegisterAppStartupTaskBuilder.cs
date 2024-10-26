namespace AppStartupTasks;

public interface IRegisterAppStartupTaskBuilder<TStartupTaskServiceType>
    where TStartupTaskServiceType : IAppStartupTask
{
    IRegisterAppStartupTaskBuilder<TStartupTaskServiceType> Add<T>()
        where T : class, TStartupTaskServiceType;

    IRegisterAppStartupTaskBuilder<TStartupTaskServiceType> Add<T>(Func<IServiceProvider, T> factory)
        where T : class, TStartupTaskServiceType;
}
