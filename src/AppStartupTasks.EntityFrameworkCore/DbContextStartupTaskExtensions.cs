namespace AppStartupTasks.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

public static class DbContextStartupTaskExtensions
{
    public static IRegisterAppStartupTaskBuilder EnsureDatabaseCreated<TDbContext>(
        this IRegisterAppStartupTaskBuilder builder)
        where TDbContext : DbContext =>
        builder.Add<EnsureDbContextCreatedTask<TDbContext>>();
}
