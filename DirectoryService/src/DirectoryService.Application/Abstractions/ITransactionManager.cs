using System.Data;
using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Application.Abstractions;

public interface ITransactionManager
{
    Task<Result<ITansactionScope, Error>> BeginTransactionAsync(
        CancellationToken cancellationToken, IsolationLevel? level = null);
    Task<UnitResult<Error>> SaveChangesAsync(CancellationToken cancellationToken);
}