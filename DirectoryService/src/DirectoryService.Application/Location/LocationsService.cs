using DirectoryService.Contracts.Location;
using Microsoft.Extensions.Logging;
using DirectoryService.Domain;
using FluentValidation;

namespace DirectoryService.Application.Location;
public class LocationsService : ILocationsService
{
    private readonly ILocationRepository _locationRepository;
    private readonly IValidator<CreateLocationDto> _validator;
    private readonly ILogger<LocationsService> _logger;

    public LocationsService(ILocationsService locationsService, 
                            ILocationRepository locationRepository, 
                            ILogger<LocationsService> logger,
                            IValidator<CreateLocationDto> validator)
    {
        _locationRepository = locationRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Guid> Create(CreateLocationDto locationDto,CancellationToken cancellationToken)
    {
        var validateResult = await _validator.ValidateAsync(locationDto, cancellationToken);
        if (!validateResult.IsValid)
        {
            throw new ValidationException(validateResult.Errors);
        }
        
        var locationId = Guid.NewGuid();

        var location = new Domain.Location()
        {
            Id = locationId,
            Address = locationDto.Address,
            TimeZone = locationDto.TimeZone,
            
        };
        
        await _locationRepository.AddAsync(location, cancellationToken); 
        
        _logger.LogInformation("Created location with {locationId}", locationId);;
        
        return locationId;
    }
}