using DirectoryService.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Tests;

public class DirectoryBaseTests : IClassFixture<DepartmentTestWebFactory>, IAsyncLifetime
{
    private readonly Func<Task> _resetDatabase;
    protected IServiceProvider Services { get; set; }

    protected DirectoryBaseTests(DepartmentTestWebFactory factory)
    {
        Services = factory.Services;
        
        _resetDatabase = factory.ResetDatabaseAsync;
    }
    
    public Task InitializeAsync()=> Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await _resetDatabase(); 
    }
    
    protected async Task<T> ExecuteInDb<T>(Func<ApplicationDbContext, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();

        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await action(db);
    }

    protected async Task ExecuteInDb(Func<ApplicationDbContext, Task> action)
    {
        await using var scope = Services.CreateAsyncScope();

        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    }
    
    protected async Task<T> ExecuteHandler<T>(Func<T, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();

        var sut = scope.ServiceProvider.GetRequiredService<T>();

        return await action(sut);
    }

}