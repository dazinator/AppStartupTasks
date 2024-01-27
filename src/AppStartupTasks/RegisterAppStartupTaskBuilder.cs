namespace AppStartupTasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public class RegisterAppStartupTaskBuilder : IRegisterAppStartupTaskBuilder
{
    private readonly IServiceCollection _serviceCollection;

    public RegisterAppStartupTaskBuilder(IServiceCollection serviceCollection) =>
        _serviceCollection = serviceCollection;

    public RegisterAppStartupTaskBuilder Add(Func<IServiceProvider, Task> initialisationScopeTasks)
    {
        Add<AsyncDelegateAppStartupTask>(sp => new AsyncDelegateAppStartupTask(sp, initialisationScopeTasks));
        return this;
    }

    public RegisterAppStartupTaskBuilder Add<T>()
        where T : class, IAppStartupTask
    {
        _serviceCollection.TryAddEnumerable(ServiceDescriptor.Describe(typeof(IAppStartupTask), typeof(T),
            ServiceLifetime.Scoped));
        return this;
    }

    public RegisterAppStartupTaskBuilder Add<T>(Func<IServiceProvider, T> factory)
        where T : class, IAppStartupTask
    {
        _serviceCollection.AddScoped(factory);
        return this;
    }
}
