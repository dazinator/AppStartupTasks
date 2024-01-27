## AppStartupTasks

Offers a clean solution to be able to register asynchronous startup tasks, and then execute those at a point wherever
you chose in your application, like:-

- In `Program.cs` before starting the main web host
- In a console app before executing a particular command
- or wherever.

This problem is blogged about previously
see https://dev-sock.netlify.app/running-async-tasks-on-app-startup-in-asp-net-core-part-1/

However the authors solution isn't what I was after because:

1. It depends on `IServer` being registered - e.g the solution will only work within the context of something like a
   WebHost server
2. There is a lack of control over then the tasks get executed precisely - in above case they will be executed then
   the `IServer` is started. In a console app or a test you may not be using `IHosting` but you are still likely to be
   using dependency injection.

## Solution

The solution is:

1. Provide a way to register start up tasks in `ServiceCollection`
2. Provide an extension method on `IServiceProvider` to execute those tasks.

This way, you can register all the tasks, and then execute them from the DI container at whatever point you chose and
the baseline requirement is just that you use DI, and no longer coupled to other infrastructure like a hosting layer.

```csharp
   [Fact]
   public async Task Docs_Usage()
   {
        var services = new ServiceCollection();

        // 1. Register the various app start up tasks in order. They won't be executed yet.
        // .. you can optionaly create extension methods on this builder, to improve fluency. 
        services.AddAppStartupTasks(builder =>
        {
            // Add some async delegate
            builder.Add(async (sp) =>
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
        
        
        // 2. At some point in your application infrastructure, once the `IServiceProvider` (container) is built,
       // execute the tasks - a convenience extension method on `IServiceProvider` is provided.       
        var sp = await services.BuildServiceProvider()
                               .StartupTasksAsync(CancellationToken.None); // if your infra provides a useful cancel token, pass it through.
   
       // Now proceed to continue spinning up your application, or starting your web host or whaatever.
   }

```
