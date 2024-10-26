// ReSharper disable once CheckNamespace
// For discoverability

namespace Microsoft.Extensions.DependencyInjection;

using AppStartupTasks;
using Extensions;

public static class AppStartupTaskRegistrationExtensions
{

    /// <summary>
    ///     Register <see cref="IAppStartupTask"/>s to be executed after the container is initialised.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IServiceCollection AddAppStartupTasks(this IServiceCollection services,
        Action<IRegisterAppStartupTaskBuilder<IAppStartupTask>> configure) =>
        AddAppStartupTasks<IAppStartupTask>(services, configure);


    /// <summary>
    ///    Register <see cref="TServiceType"/>s to be executed after the container is initialised.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IServiceCollection AddAppStartupTasks<TServiceType>(this IServiceCollection services,
        Action<IRegisterAppStartupTaskBuilder<TServiceType>> configure)
    where TServiceType: IAppStartupTask
    {
        services.TryAddSingleton(typeof(IAppStartupTaskExecutor<TServiceType>), typeof(AppStartupTaskExecutor<TServiceType>)); //<TServiceType>, AppStartupTaskExecutor>();
       // services.TryAddSingleton<IAppStartupTaskExecutor<TServiceType>, AppStartupTaskExecutor<TServiceType>>();
        var builder = new RegisterAppStartupTaskBuilder<TServiceType>(services);
        configure?.Invoke(builder);
        return services;
    }

    /// <summary>
    ///  Executes all startup tasks registered as <see cref="IAppStartupTask"/> in a new temporary DI scope.
    /// </summary>
    /// <returns></returns>
    public static Task<IServiceProvider> StartupTasksAsync(
        this IServiceProvider serviceProvider, CancellationToken cancellationToken) =>
        StartupTasksAsync<IAppStartupTask>(serviceProvider, cancellationToken);

    /// <summary>
    ///  Executes all startup tasks registered as <see cref="TServiceType"/> in a new temporary DI scope.
    /// </summary>
    /// <returns></returns>
    public static async Task<IServiceProvider> StartupTasksAsync<TServiceType>(
        this IServiceProvider serviceProvider, CancellationToken cancellationToken)
        where TServiceType: IAppStartupTask
    {
        await serviceProvider.ExecuteInNewScope(async sp =>
        {
            var executor = sp.GetRequiredService<IAppStartupTaskExecutor<TServiceType>>();
            await executor.ExecuteAsync(cancellationToken);
        });
        return serviceProvider;
    }

    private static async Task ExecuteInNewScope(this IServiceProvider serviceProvider,
        Func<IServiceProvider, Task> initialisationScopeTasks)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        await initialisationScopeTasks(scope.ServiceProvider);
    }
}
