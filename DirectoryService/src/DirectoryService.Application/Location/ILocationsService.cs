using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using Shared;

namespace DirectoryService.Application;

public interface ILocationsService
{
    Task<Result<Guid,Error>> Create(CreateLocationDto locationDto, CancellationToken cancellationToken);
}