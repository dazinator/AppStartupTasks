// ReSharper disable once CheckNamespace
// For discoverability

namespace Microsoft.Extensions.DependencyInjection;

using AppStartupTasks;
using Extensions;

public static class AppStartupTaskRegistrationExtensions
{
    /// <summary>
    ///     Builds the service provider, but before returning it, creates a new temporary scope and executes the callback
    ///     within that new scope for you to do any one time setup.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IServiceCollection AddAppStartupTasks(this IServiceCollection services,
        Action<IRegisterAppStartupTaskBuilder> configure)
    {
        services.TryAddSingleton<IAppStartupTaskExecutor, AppStartupTaskExecutor>();
        var builder = new RegisterAppStartupTaskBuilder(services);
        configure?.Invoke(builder);
        return services;
    }

    /// <summary>
    ///     Executes initialisation registered using <see cref="AddAppStartupTasks" /> in a new temporary DI scope.
    /// </summary>
    /// <returns></returns>
    public static async Task<IServiceProvider> StartupTasksAsync(
        this IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        await serviceProvider.ExecuteInNewScope(async sp =>
        {
            var executor = sp.GetRequiredService<IAppStartupTaskExecutor>();
            await executor.ExecuteAsync(cancellationToken);
        });
        return serviceProvider;
    }

    public static async Task ExecuteInNewScope(this IServiceProvider serviceProvider,
        Func<IServiceProvider, Task> initialisationScopeTasks)
    {
        await using (var scope = serviceProvider.CreateAsyncScope())
        {
            await initialisationScopeTasks(scope.ServiceProvider);
        }
    }
}
