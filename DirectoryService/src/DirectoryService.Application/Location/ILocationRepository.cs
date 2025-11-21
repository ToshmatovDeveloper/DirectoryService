using DirectoryService.Domain;

namespace DirectoryService.Application;

public interface ILocationRepository
{
    Task<Guid> AddAsync(Location location, CancellationToken cancellationToken);
}