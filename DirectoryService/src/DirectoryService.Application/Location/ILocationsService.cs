using DirectoryService.Contracts;

namespace DirectoryService.Application;

public interface ILocationsService
{
    Task<Guid> Create(CreateLocationDto locationDto, CancellationToken cancellationToken);
}