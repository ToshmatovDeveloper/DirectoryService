namespace DirectoryService.Application.Location;
using DirectoryService.Domain;

public interface ILocationRepository
{
    Task<Guid> AddAsync(Location location, CancellationToken cancellationToken);
    
    
}