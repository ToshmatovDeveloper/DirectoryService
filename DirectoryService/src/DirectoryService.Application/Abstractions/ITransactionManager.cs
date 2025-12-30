using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using Shared;

namespace DirectoryService.Infrastructure.Database;

public interface ITransactionManager
{
    Task<Result<ITansactionScope, Error>> BeginTransactionAsync(CancellationToken cancellationToken);
    Task<UnitResult<Error>> SaveChangesAsync(CancellationToken cancellationToken);
}