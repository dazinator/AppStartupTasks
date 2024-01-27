// ReSharper disable once CheckNamespace

namespace Microsoft.Extensions.DependencyInjection;

using AppStartupTasks.EntityFrameworkCore;
using AppStartupTasks.EntityFrameworkCore.Sqlite;
using EntityFrameworkCore;

public static class SqliteEfExtensions
{
    /// <summary>
    ///     Registers an InMemory <see cref="TDbContext" /> and some AppStartupTasks to ensure the InMemory sqlite connection
    ///     lasts for as long as the container lifetime,
    ///     to prevent the database from dropping when the connection would ordinarily be closed by EF.
    ///     You should call <see cref="AppStartupTaskRegistrationExtensions.StartupTasksAsync" /> after building the DI
    ///     container to ensure startup tasks essential for the management of the in memory sqlite connection are run.
    /// </summary>
    /// <param name="services"></param>
    /// <typeparam name="TDbContext"></typeparam>
    /// <returns></returns>
    public static IServiceCollection AddInMemorySqliteDbContext<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        services.AddSingleton<SqliteConnectionService>(sp => new SqliteConnectionService("DataSource=:memory:"));
        services.AddDbContext<TDbContext>((sp, a) =>
        {
            var conn = sp.GetRequiredService<SqliteConnectionService>();
            a.UseSqlite(conn.GetConnection());
        });

        services.AddAppStartupTasks(b =>
        {
            b.EnsureSqliteInMemoryConnectionOpen()
                .EnsureDatabaseCreated<TDbContext>();
        });

        return services;
    }
}
