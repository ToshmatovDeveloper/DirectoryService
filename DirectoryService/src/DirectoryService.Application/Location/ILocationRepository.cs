using CSharpFunctionalExtensions;
using DirectoryService.Domain.ValueObjects;
using Shared;

namespace DirectoryService.Application.Location;

public interface ILocationRepository
{
    Task<Result<Guid,Error>> AddAsync(Domain.Location location, CancellationToken cancellationToken);
}