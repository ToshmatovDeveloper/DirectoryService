using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Location;
using DirectoryService.Application.Location.Create;
using DirectoryService.Application.Location.Query;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Create;
using DirectoryService.Contracts.Get;
using DirectoryService.Contracts.GetRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Results;

namespace DirectoryService.Presentation;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly ILogger<LocationsController> _logger;

    public LocationsController(ILocationsService locationsService, 
                               ICommandHandler<Guid, CreateLocationRequest> commandHandler, 
                               ILogger<LocationsController> logger)
    {
        _logger = logger;
    }

    [HttpGet("{locationId:guid}")]
    public async Task<ActionResult<GetLocationByIdDto>> GetById(
        [FromRoute] Guid locationId,
        [FromServices] GetLocationByIdHandler  handler,
        CancellationToken cancellationToken)
    {
        var @location = await handler.Handle(new GetLocationByIdRequest(locationId), cancellationToken); 
        return Ok(@location);
    }
    
    [HttpGet()]
    public async Task<ActionResult<GetLocationsDto>> Get(
        [FromQuery] GetLocationRequest request,
        [FromServices] GetLocationHandler  handler,
        CancellationToken cancellationToken)
    {
        var locations = await handler.Handle(request, cancellationToken);
        return Ok();
    }
    
    [HttpPost]
    public async Task<EndpointResult<Guid>> Create([FromServices] ICommandHandler<CreateLocationDto , 
                                                   CreateLocationRequest> handler,
                                                   CreateLocationDto request,
                                                   CancellationToken cancellationToken)
    {
        _logger.LogDebug("Вызван CreateLocation с данными: {@locationDto}", request);        
        
        var command = new CreateLocationRequest(request);
        
        var locationId = await handler.Handle(command,cancellationToken);

        if (locationId.IsFailure)
        {
            _logger.LogWarning("Ошибка при создании локации: {Error}", locationId.Error);
            return new EndpointResult<Guid>(locationId.Error);
        }
        
        _logger.LogInformation("Локация создана. Id = {Id}", locationId.Value);
        
        return locationId;
    }
}