using DirectoryService.Application.Location;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Location;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<Guid> Create(CreateLocationDto locationDto,CancellationToken cancellationToken)
    {
        var locationId = await _locationsService.Create(locationDto,cancellationToken);
        return locationId;
    }
}