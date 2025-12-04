using CSharpFunctionalExtensions;
using DirectoryService.Application;
using DirectoryService.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared;

namespace DirectoryService.Presentation.Location;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly ILocationsService  _locationsService;
    private readonly ILogger<LocationsController> _logger;

    public LocationsController(ILocationsService locationsService)
    {
        _locationsService = locationsService;
    }

    [HttpPost]
    public async Task<EndpointResult<Guid>> Create(CreateLocationDto locationDto,
                                                   CancellationToken cancellationToken)
    {
        _logger.LogDebug("Вызван CreateLocation с данными: {@locationDto}", locationDto);        
        
        var locationId = await _locationsService.Create(locationDto,cancellationToken);

        if (locationId.IsFailure)
        {
            _logger.LogWarning("Ошибка при создании локации: {Error}", locationId.Error);
            return new EndpointResult<Guid>(locationId.Error);
        }
        
        _logger.LogInformation("Локация создана. Id = {Id}", locationId.Value);
        
        return locationId;
    }
}