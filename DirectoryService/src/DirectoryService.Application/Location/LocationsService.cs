using System.ComponentModel.DataAnnotations;
using DirectoryService.Contracts;
using DirectoryService.Domain;
using Microsoft.Extensions.Logging;
using TimeZone = DirectoryService.Domain.TimeZone;

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

    public async Task<Guid> Create(CreateLocationDto locationDto, CancellationToken cancellationToken)
    {
        var validateResult = await _validator.ValidateAsync(locationDto, cancellationToken);
        
        if(!validateResult.IsValid)
        {
            throw new ValidationException(validateResult.Errors.ToString());
        }

        var locationId = Guid.NewGuid();

        var location = new Location(
            locationId,
            new Name(locationDto.Name),
            new Address(locationDto.Address.Country, locationDto.Address.City, locationDto.Address.Street),
            new TimeZone(locationDto.TimeZone));
        
        await _locationRepository.AddAsync(location, cancellationToken);
        
        _logger.LogInformation("Location created with id {locationId}", locationId);
        
        return locationId;
    }
}