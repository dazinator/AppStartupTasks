namespace Tests;

using AppStartupTasks;

[Documentation]
public class UsageTests
{
    [Fact]
    public async Task Docs_Usage()
    {
        var services = new ServiceCollection();

        // 1. Register the various app start up tasks in order. They won't be executed yet.
        // .. using the IRegisterAppStartupTaskBuilder builder - you can optionaly create extension methods for this to improve fluency.
        services.AddAppStartupTasks(builder =>
        {
            // Add some async delegate
            builder.Add(async sp =>
                {
                    // var someDep = sp.GetRequiredService<ILogger<Program>>()>()
                    await Task.Delay(100);
                })
                .Add<WriteSomethingToConsole>() // register your own implementation of IAppStartupTask - supports DI.
                .Add<WriteMessageToConsole>(sp =>
                    new WriteMessageToConsole("Yo")); // shows registering your IAppStartupTask using a factory method
        });

        // Also you can bypass the builder and just register IAppStartupTask implementation directly.
        services.AddScoped<IAppStartupTask, WriteSomethingToConsole>();


        // 2. At some point in your application, once the `IServiceProvider` is built, execute the tasks!
        //    You can use the .StartupTasksAsync extennsion method on `IServiceProvider` to do this.
        var sp = await services.BuildServiceProvider()
            .StartupTasksAsync(CancellationToken.None);
    }

    public class WriteSomethingToConsole : IAppStartupTask
    {
        public Task InitialiseAsync(CancellationToken ct)
        {
            Console.WriteLine("Hey");
            return Task.CompletedTask;
        }
    }

    public class WriteMessageToConsole : IAppStartupTask
    {
        private readonly string _message;

        public WriteMessageToConsole(string message) => _message = message;

        public Task InitialiseAsync(CancellationToken ct)
        {
            Console.WriteLine(_message);
            return Task.CompletedTask;
        }
    }
}
