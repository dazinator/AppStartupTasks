namespace AppStartupTasks;

public interface IAppStartupTaskExecutor<TServiceType>
{
    Task ExecuteAsync(CancellationToken ct);
}

public interface IAppStartupTaskExecutor: IAppStartupTaskExecutor<IAppStartupTask>
{

}
