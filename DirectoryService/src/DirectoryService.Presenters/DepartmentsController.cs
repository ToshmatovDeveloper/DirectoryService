using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Department.Create;
using DirectoryService.Application.Location;
using DirectoryService.Application.Location.Create;
using DirectoryService.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Results;

namespace DirectoryService.Presentation;


[ApiController]
[Route("api/[controller]")]
public class DepartmentsController : ControllerBase
{
    private readonly ILogger<DepartmentsController> _logger;

    public DepartmentsController(
        ICommandHandler<Guid, CreateDepartmentRequest> commandHandler, 
        ILogger<DepartmentsController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<EndpointResult<Guid>> Create(
        [FromServices] ICommandHandler<CreateDepartmentDto, CreateDepartmentRequest> handler,
        CreateDepartmentDto request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Вызван CreateDepartment с данными: {@locationDto}", request);        
        
        var command = new CreateDepartmentRequest(request);
        
        var departmentId = await handler.Handle(command,cancellationToken);

        if (departmentId.IsFailure)
        {
            _logger.LogWarning("Ошибка при создании отделении: {Error}", departmentId.Error);
            return new EndpointResult<Guid>(departmentId.Error);
        }
        
        _logger.LogInformation("Отдел создан. Id = {Id}", departmentId.Value);
        
        return departmentId;
    }
}