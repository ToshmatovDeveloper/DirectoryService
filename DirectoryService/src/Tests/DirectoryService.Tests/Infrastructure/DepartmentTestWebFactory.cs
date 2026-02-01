using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using DirectoryService.Application.Department.Create;
using DirectoryService.Infrastructure;
using DirectoryService.Infrastructure.Factory;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace DirectoryService.Tests;

public class DepartmentTestWebFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _container;
    
    public DepartmentTestWebFactory()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase($"test_db_{Guid.NewGuid()}") 
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();
    }
    
    
    
    private Respawner _respawner = null!;

    private DbConnection _dbConnection = null!;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            var connectionString = _container.GetConnectionString();
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "ConnectionStrings:Database", connectionString },
                { "ConnectionStrings:DirectoryServiceDb", connectionString }
            });
        });
        
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.RemoveAll<ApplicationDbContext>();
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(_container.GetConnectionString()));
            
            services.RemoveAll<ISqlConnectionFactory>();
            services.AddScoped<ISqlConnectionFactory>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return new SqlConnectionFactory(configuration);
            });
            
            services.AddScoped<CreateDepartmentHandler>();
        });
    }
    public async Task InitializeAsync()
    {
        await _container.StartAsync();
        
        await using var scope = Services.CreateAsyncScope();
        var dbContext  = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
        
        _dbConnection = new NpgsqlConnection(_container.GetConnectionString());
        
        await InitializeRespawner();
    }

    public new async Task DisposeAsync()
    {
        await _container.StopAsync();
        await _container.DisposeAsync();

        await _dbConnection.CloseAsync();
        await _dbConnection.DisposeAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        
        await using var connection = new NpgsqlConnection(_container.GetConnectionString());
        await connection.OpenAsync();  
    
        await _respawner.ResetAsync(connection);  
    }
    
    private async Task InitializeRespawner()
    {
        await using var connection = new NpgsqlConnection(_container.GetConnectionString());
        await connection.OpenAsync(); 
    
        _respawner = await Respawner.CreateAsync(
            connection, 
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["public"]
            });
    }
}