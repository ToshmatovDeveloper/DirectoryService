using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Position.Create;
using DirectoryService.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Results;

namespace DirectoryService.Presentation;

[ApiController]
[Route("api/[controller]")]
public class PositionsController : ControllerBase
{
    private readonly ILogger<PositionsController> _logger;

    public PositionsController(
        ICommandHandler<Guid, CreatePositionRequest> commandHandler, 
        ILogger<PositionsController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<EndpointResult<Guid>> Create(
        [FromServices] ICommandHandler<CreateDepartmentDto, CreatePositionRequest> handler,
        CreatePostionDto request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Вызван CreatePosition с данными: {@locationDto}", request);        
        
        var command = new CreatePositionRequest(request);
        
        var positionId = await handler.Handle(command,cancellationToken);

        if (positionId.IsFailure)
        {
            _logger.LogWarning("Ошибка при создании позиции: {Error}", positionId.Error);
            return new EndpointResult<Guid>(positionId.Error);
        }
        
        _logger.LogInformation("Позиция создана. Id = {Id}", positionId.Value);
        
        return positionId;
    }
}