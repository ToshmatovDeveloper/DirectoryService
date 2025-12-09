using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Application.Location;

public interface ILocationRepository
{
    Task<Result<Guid,Error>> AddAsync(Domain.Location location, CancellationToken cancellationToken);
}