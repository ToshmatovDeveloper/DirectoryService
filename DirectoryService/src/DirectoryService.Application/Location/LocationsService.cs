using CSharpFunctionalExtensions;
using DirectoryService.Contracts;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using Shared;
using TimeZone = DirectoryService.Domain.ValueObjects.TimeZone;

namespace DirectoryService.Application;

public class LocationsService : ILocationsService
{
    private readonly ILocationRepository _locationRepository;
    private readonly CreateLocationValidator _validator;
    private readonly ILogger<LocationsService> _logger;

    public LocationsService(
        ILogger<LocationsService> logger,
        ILocationRepository locationRepository,
        CreateLocationValidator createLocationValidator)
    {
        _locationRepository = locationRepository;
        _validator = createLocationValidator;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Create(CreateLocationDto locationDto, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Creating new location");
        
        var validationResult = await _validator.ValidateAsync(locationDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation Failed");
            
            return GeneralErrors.ValueIsInvalid("Location");
        }

        var locationId = Guid.NewGuid();

        var location = new Location(
            locationId,
            new Name(locationDto.Name),
            new Address(locationDto.Address.Country, locationDto.Address.City, locationDto.Address.Street),
            new TimeZone(locationDto.TimeZone));

        Result<Guid, Error> result = await _locationRepository.AddAsync(location, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error;
        }

        _logger.LogInformation("Location created with id {locationId}", locationId);

        return locationId;
    }
}