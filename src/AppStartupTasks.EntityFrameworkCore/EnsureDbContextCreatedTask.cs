namespace AppStartupTasks.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

public class EnsureDbContextCreatedTask<TDbContext> : IAppStartupTask
    where TDbContext : DbContext
{
    private readonly TDbContext _dbContext;

    public EnsureDbContextCreatedTask(TDbContext dbContext) => _dbContext = dbContext;

    public async Task InitialiseAsync(CancellationToken ct) => await _dbContext.Database.EnsureCreatedAsync(ct);
}
