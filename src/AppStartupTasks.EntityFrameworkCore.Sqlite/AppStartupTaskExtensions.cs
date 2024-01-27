namespace AppStartupTasks.EntityFrameworkCore.Sqlite;

public static class SqliteAppStartupTaskExtensions
{
    /// <summary>
    ///     Registers a startup task <see cref="SqliteInMemoryConnectionOpenTask" />
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IRegisterAppStartupTaskBuilder EnsureSqliteInMemoryConnectionOpen(
        this IRegisterAppStartupTaskBuilder builder) => builder.Add<SqliteInMemoryConnectionOpenTask>();
}
