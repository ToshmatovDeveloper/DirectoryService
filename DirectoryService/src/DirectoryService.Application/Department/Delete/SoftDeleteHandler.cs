using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Locations;
using DirectoryService.Application.Position;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Shared;

namespace DirectoryService.Application.Department.Delete;

public class SoftDeleteHandler
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly IPositionRepository _positionRepository;
    private readonly ILocationRepository _locationRepository;
    private readonly ITransactionManager _transactionManager;
    private readonly ILogger<SoftDeleteHandler> _logger;

    public SoftDeleteHandler(IDepartmentRepository departmentRepository, ITransactionManager transactionManager, ILogger<SoftDeleteHandler> logger, IPositionRepository positionRepository, ILocationRepository locationRepository)
    {
        _departmentRepository = departmentRepository;
        _transactionManager = transactionManager;
        _logger = logger;
        _positionRepository = positionRepository;
        _locationRepository = locationRepository;
    }

    public async Task<Result<Domain.Department, Error>> Handle(Guid id, CancellationToken cancellationToken)
    {
        var transactionScopeResult = await _transactionManager.BeginTransactionAsync(cancellationToken: cancellationToken);
        if (transactionScopeResult.IsFailure)
            return transactionScopeResult.Error;

        using var transactionScope = transactionScopeResult.Value;

        var lockDepartmentResult = await _departmentRepository.GetByIdWithLockAsync(id, cancellationToken);
        if (lockDepartmentResult.IsFailure)
        {
            transactionScope.Rollback();
            return lockDepartmentResult.Error;
        }

        var department = lockDepartmentResult.Value;
        string oldPath = department.Path.Value;

        var lockDescendantsResult = await _departmentRepository.LockChildrenByPath(oldPath, cancellationToken);
        if (lockDescendantsResult.IsFailure)
        {
            transactionScope.Rollback();
            return lockDescendantsResult.Error;
        }

        department.SoftDelete();

        string deletionPrefix = "del_";

        var updateDepPathResult = await _departmentRepository
            .MarkDepartmentAsDeleted(deletionPrefix, department.Id, cancellationToken);
        if (updateDepPathResult.IsFailure)
        {
            _logger.LogError("Error when update path of department:{department}.", department.Id);
            transactionScope.Rollback();
            return updateDepPathResult.Error;
        }

        string newPath = department.Path.Value;

        var updateDescendantsPathResult = await _departmentRepository.UpdateAllDescendantsPath(
            oldPath,
            newPath,
            department.Id,
            cancellationToken);
        if (updateDescendantsPathResult.IsFailure)
        {
            _logger.LogError("Error when update path descendants of department:{department}", department.Id);
            transactionScope.Rollback();
            return updateDescendantsPathResult.Error;
        }

        var updatedPositionsResult = await _positionRepository.SoftDeleteUniqDepRelatedPositions(department.Id,
            cancellationToken);
        if (updatedPositionsResult.IsFailure)
        {
            transactionScope.Rollback();
            return updatedPositionsResult.Error;
        }

        var updatedLocationsResult = await _locationRepository.SoftDeleteUniqDepRelatedLocations(department.Id,
            cancellationToken);
        if (updatedLocationsResult.IsFailure)
        {
            transactionScope.Rollback();
        }

        await _transactionManager.SaveChangesAsync(cancellationToken);

        var commitedResult = transactionScope.Commit();
        if (commitedResult.IsFailure)
        {
            return commitedResult.Error;
        }

        _logger.LogInformation("Department: {DepartmentId} was soft deleted with descendants.", department.Id);

        return department;
    }
}