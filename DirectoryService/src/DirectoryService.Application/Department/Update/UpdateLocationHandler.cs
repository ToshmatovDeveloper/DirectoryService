using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Location;
using DirectoryService.Application.Validation;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared;

namespace DirectoryService.Application.Department.Update;

public class UpdateLocationHandler : ICommandHandler<Guid, UpdateLocationRequest>
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly IValidator<UpdateLocationRequest> _validator;
    private readonly ITransactionManager _transactionManager;
    private readonly ILogger<UpdateLocationHandler> _logger;

    public UpdateLocationHandler(
        IDepartmentRepository departmentRepository,
        ILocationRepository locationRepository,
        IValidator<UpdateLocationRequest> validator,
        ILogger<UpdateLocationHandler> logger,
        ITransactionManager transactionManager)
    {
        _validator = validator;
        _logger = logger;
        _transactionManager = transactionManager;
        _departmentRepository = departmentRepository;
        _locationRepository = locationRepository;
    }

    public async Task<Result<Guid, Error>> Handle(
        UpdateLocationRequest request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogError("Invalid request");
            return validationResult.ToError();
        }

        var departmentId = Guid.NewGuid();

        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(cancellationToken);

        if (transactionScopeResult.IsFailure)
        {
            return transactionScopeResult.Error;
        }

        using var transactionScope = transactionScopeResult.Value;

        var department = await _departmentRepository
            .GetByIdWithLocationAsync(departmentId, cancellationToken);

        if (department.IsFailure)
        {
            transactionScope.Rollback();
            return department.Error;
        }

        if (!department.Value.IsActive)
        {
            transactionScope.Rollback();
            return department.Error;
        }

        var location = await _locationRepository.CheckActiveLocationsDyId(
            request.LocationsId.Select(LocationId.Create), cancellationToken);

        if (location.IsFailure)
        {
            transactionScope.Rollback();
            return department.Error;
        }

        List<DepartmentLocation> departmentLocations = [];

        foreach (var locationId in departmentLocations.ToList())
        {
            var departmentLocation = new DepartmentLocation(
                departmentId, locationId.DepartmentId);

            departmentLocations.Add(departmentLocation);
        }

        department.Value.SetLocations(departmentLocations);

        await _transactionManager.SaveChangesAsync(cancellationToken);

        transactionScope.Commit();
        
        return departmentId;
    }
}