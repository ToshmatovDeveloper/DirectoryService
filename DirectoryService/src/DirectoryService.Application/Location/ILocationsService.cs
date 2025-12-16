using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using DirectoryService.Domain.ValueObjects;
using Shared;

namespace DirectoryService.Application.Location;

public interface ILocationsService
{
    Task<Result<Guid, Error>> Create(CreateLocationDto locationDto, CancellationToken cancellationToken);
}