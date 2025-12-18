using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Application.Position;

public interface IPositionRepository
{
    Task<Result<Guid, Error>> AddAsync(Domain.Position position, CancellationToken cancellationToken);
    Task<Result<bool, Error>> AlreadyExistPosition(Domain.Position position, CancellationToken cancellationToken);

}