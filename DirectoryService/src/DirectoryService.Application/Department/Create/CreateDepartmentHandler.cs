using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Validation;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared;

namespace DirectoryService.Application.Department.Create;

public class CreateDepartmentHandler : ICommandHandler<Guid, CreateDepartmentRequest>
{
    private readonly IDepartmentRepository _repository;
    private readonly IValidator<CreateDepartmentRequest> _validator;
    private readonly ILogger<CreateDepartmentHandler> _logger;

    public CreateDepartmentHandler(IDepartmentRepository repository, IValidator<CreateDepartmentRequest> validator,
        ILogger<CreateDepartmentHandler> logger)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(CreateDepartmentRequest request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogError("Invalid DepartmentDto");
            return validationResult.ToError();
        }

        var nameResult = Name.Create(request.CreateDepartmentDto.Name);
        if (nameResult.IsFailure)
        {
            _logger.LogError("Invalid DepartmentDto.Name");
            return nameResult.Error;
        }

        var identifierResult = Identifier.Create(request.CreateDepartmentDto.Identifier);
        if (identifierResult.IsFailure)
        {
            _logger.LogError("Invalid DepartmentDto.Identifier");
            return identifierResult.Error;
        }

        var departmentId = new DepartmentId(Guid.NewGuid());


        var departmentLocations = request.CreateDepartmentDto.LocationIds
            .Select(locationId => new DepartmentLocation(
                new DepartmentId(Guid.NewGuid()),
                new LocationId(locationId)))
            .ToList();


        Result<Domain.Department, Error> departmentResult;
        if (request.CreateDepartmentDto.ParentId == null)
        {
            departmentResult = Domain.Department.CreateParent(
                nameResult.Value,
                identifierResult.Value,
                departmentLocations,
                departmentId);
        }
        else
        {
            var parentResult = await _repository.GetByIdAsync(request.CreateDepartmentDto.ParentId, cancellationToken);
            if (parentResult.IsFailure)
            {
                _logger.LogError("Parent department not found");
                return parentResult.Error;
            }

            departmentResult = Domain.Department.CreateChild(
                nameResult.Value,
                identifierResult.Value,
                parentResult.Value,
                departmentLocations,
                departmentId);
        }

        if (departmentResult.IsFailure)
        {
            _logger.LogError("Invalid DepartmentDto.Department");
            return departmentResult.Error;
        }

        await _repository.AddAsync(departmentResult.Value, cancellationToken);

        _logger.LogInformation("Department created successfully with id {departmentId}", departmentId);

        return (Result<Guid, Error>)departmentId.Value!;
    }
}