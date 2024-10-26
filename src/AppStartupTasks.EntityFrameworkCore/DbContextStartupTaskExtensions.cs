namespace AppStartupTasks.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

public static class DbContextStartupTaskExtensions
{
    public static IRegisterAppStartupTaskBuilder<IAppStartupTask> EnsureDatabaseCreated<TDbContext>(
        this IRegisterAppStartupTaskBuilder<IAppStartupTask> builder)
        where TDbContext : DbContext =>
        builder.Add<EnsureDbContextCreatedTask<TDbContext>>();
}
