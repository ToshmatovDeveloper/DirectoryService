using System.Data;
using System.Data.Common;

namespace DirectoryService.Infrastructure.Factory;

public interface ISqlConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
}