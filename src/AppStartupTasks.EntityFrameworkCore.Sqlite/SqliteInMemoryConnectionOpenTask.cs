namespace AppStartupTasks.EntityFrameworkCore.Sqlite;

/// <summary>
///     Responsibility: Ensure to open the sqlite in memory connection before using the DbContext in the application, so we
///     can keep it open for the full life of the DI container.
///     If we don't do this, the In memory sql lite database is lost as soon as EF Core DbContext closes the connection. EF
///     Core DbContext won't open or close a connection if it sees it's already open onn first use.
/// </summary>
public class SqliteInMemoryConnectionOpenTask : IAppStartupTask
{
    private readonly SqliteConnectionService _connection;

    public SqliteInMemoryConnectionOpenTask(SqliteConnectionService connection) => _connection = connection;

    public async Task InitialiseAsync(CancellationToken ct) => await _connection.OpenAsync();
}
