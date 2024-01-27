namespace AppStartupTasks.EntityFrameworkCore.Sqlite;

using Microsoft.Data.Sqlite;

public class SqliteConnectionService : IDisposable, IAsyncDisposable
{
    private readonly SqliteConnection _sqlite;

    public SqliteConnectionService(string connectionString) => _sqlite = new SqliteConnection(connectionString);

    public async ValueTask DisposeAsync() => await _sqlite.DisposeAsync();

    public void Dispose() => _sqlite.Dispose();

    public SqliteConnection GetConnection() => _sqlite;

    //  b.UseSqlite(_sqlite);
    public void Open() => _sqlite.Open();

    public Task OpenAsync() => _sqlite.OpenAsync();
}
