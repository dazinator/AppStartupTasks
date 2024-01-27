// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceProviderExtensions
{
    public static async Task ExecuteInNewScope<TService>(
        this IServiceProvider serviceProvider,
        Func<TService, Task> execute)
        where TService : notnull
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var service = scope.ServiceProvider.GetRequiredService<TService>();
        await execute(service);
    }
}
