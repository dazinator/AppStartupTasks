namespace AppStartupTasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public class RegisterAppStartupTaskBuilder<TStartupTaskServiceType> : IRegisterAppStartupTaskBuilder<TStartupTaskServiceType>
where TStartupTaskServiceType: IAppStartupTask
{
    private readonly IServiceCollection _serviceCollection;

    public RegisterAppStartupTaskBuilder(IServiceCollection serviceCollection) =>
        _serviceCollection = serviceCollection;

    public IRegisterAppStartupTaskBuilder<TStartupTaskServiceType> Add<T>()
        where T : class, TStartupTaskServiceType
    {
        _serviceCollection.TryAddEnumerable(ServiceDescriptor.Describe(typeof(TStartupTaskServiceType), typeof(T),
            ServiceLifetime.Scoped));
        return this;
    }

    public IRegisterAppStartupTaskBuilder<TStartupTaskServiceType> Add<T>(Func<IServiceProvider, T> factory)
        where T : class, TStartupTaskServiceType
    {
        _serviceCollection.AddScoped(factory);
        return this;
    }
}
