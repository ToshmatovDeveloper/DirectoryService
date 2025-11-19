using DirectoryService.Contracts;
using DirectoryService.Contracts.Location;

namespace DirectoryService.Application.Location;

public interface ILocationsService
{
    Task<Guid> Create(CreateLocationDto locationDto,CancellationToken cancellationToken);
}