namespace Tests.EFCore.Sqlite;

using Microsoft.EntityFrameworkCore;

[UnitTest]
[UsesVerify]
public class AddInMemorySqliteDbContextTests
{
    public AddInMemorySqliteDbContextTests(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper;
        DefaultServices = new ServiceCollection();
        DefaultServices.AddLogging(builder => builder.AddXUnit(OutputHelper));
    }


    public ITestOutputHelper OutputHelper { get; }

    public IServiceCollection DefaultServices { get; }

    [Fact]
    public async Task SaveChanges_DifferentScopes_DatabasePersists()
    {
        var sp = await DefaultServices
            .AddInMemorySqliteDbContext<BloggingContext>()
            .BuildServiceProvider()
            .StartupTasksAsync(default);

        await sp.ExecuteInNewScope<BloggingContext>(async dbContext =>
        {
            dbContext.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
            await dbContext.SaveChangesAsync();
        });

        await sp.ExecuteInNewScope<BloggingContext>(async dbContext =>
        {
            var count = await dbContext.Blogs.CountAsync();
            count.ShouldBe(1);
        });
    }
}
