using System.Data;

namespace DirectoryService.Application.Abstractions;

public interface ISqlConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}