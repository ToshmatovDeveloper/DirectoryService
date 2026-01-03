using System.Data;
using System.Data.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace DirectoryService.Infrastructure.Factory;

public class SqlConnectionFactory :IDisposable, IAsyncDisposable, ISqlConnectionFactory
{
    private readonly NpgsqlDataSource _dataSource;
    public SqlConnectionFactory(IConfiguration configuration)
    {
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("Database"));

        dataSourceBuilder.UseLoggerFactory(CreateLoggerFactory());
        
        _dataSource = dataSourceBuilder.Build();
    }
    
    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
    {
        return await _dataSource.OpenConnectionAsync(cancellationToken);
    }
    
    private ILoggerFactory CreateLoggerFactory() =>
        LoggerFactory.Create(builder => { builder.AddConsole(); });

    public void Dispose()
    {
        _dataSource.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _dataSource.DisposeAsync();
    }
}