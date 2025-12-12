using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Validation;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Shared;
using Path = DirectoryService.Domain.ValueObjects.Path;

namespace DirectoryService.Application.Department.Create;

public class CreateDepartmentHandler : ICommandHandler<Guid, CreateDepartmentRequest>
{
    private readonly IDepartmentRepository _repository;
    private readonly IValidator<CreateDepartmentRequest> _validator;
    private readonly ILogger<CreateDepartmentHandler> _logger;

    public CreateDepartmentHandler(IDepartmentRepository repository, IValidator<CreateDepartmentRequest> validator, ILogger<CreateDepartmentHandler> logger)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreateDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _validator
            .ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Validation failed");

            return validationResult.ToError();
        }

        var departmentId = Guid.NewGuid();

        var department = new Domain.Department(
            departmentId,
            new Name(request.CreateDepartmentDto.Name),
            new Identifier(request.CreateDepartmentDto.Identifier),
            new Guid?(request.CreateDepartmentDto.ParentId),
            request.CreateDepartmentDto.LocationIds.Distinct().ToArray()
        );

        Result<Guid, Error> result = await _repository
            .AddAsync(department, cancellationToken);
        
        if (result.IsFailure)
        {
            return result.Error;
        }
        
        _logger.LogInformation("Location created with id {locationId}", departmentId);

        return departmentId;
    }
}