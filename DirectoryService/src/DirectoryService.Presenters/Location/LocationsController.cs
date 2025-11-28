using CSharpFunctionalExtensions;
using DirectoryService.Application;
using DirectoryService.Contracts;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace DirectoryService.Presentation.Location;

[ApiController]
[Route("api/locations")]
public class LocationsController : ControllerBase
{
    private readonly ILocationsService  _locationsService;

    public LocationsController(ILocationsService locationsService)
    {
        _locationsService = locationsService;
    }

    [HttpPost]
    public async Task<EndpointResult<Guid>> Create(CreateLocationDto locationDto,CancellationToken cancellationToken)
    {
        var locationId = await _locationsService.Create(locationDto,cancellationToken);
        return locationId;
    }
}