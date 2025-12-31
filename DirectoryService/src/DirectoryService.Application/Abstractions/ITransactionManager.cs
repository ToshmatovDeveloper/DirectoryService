using System.Data;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using Shared;

namespace DirectoryService.Infrastructure.Database;

public interface ITransactionManager
{
    Task<Result<ITansactionScope, Error>> BeginTransactionAsync(
        CancellationToken cancellationToken, IsolationLevel? level = null);
    Task<UnitResult<Error>> SaveChangesAsync(CancellationToken cancellationToken);
}