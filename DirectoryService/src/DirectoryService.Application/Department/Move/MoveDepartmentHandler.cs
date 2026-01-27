using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using Microsoft.Extensions.Logging;
using Shared;
using Path = DirectoryService.Domain.ValueObjects.Path;

namespace DirectoryService.Application.Department.Move;

public class MoveDepartmentHandler
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ITransactionManager  _transactionManager;
    private readonly ILogger<MoveDepartmentHandler> _logger;
    
    public MoveDepartmentHandler(IDepartmentRepository departmentRepository, ITransactionManager transactionManager, ILogger<MoveDepartmentHandler> logger)
    {
        _departmentRepository = departmentRepository;
        _transactionManager = transactionManager;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        Guid departmentId, 
        MoveDepartmentRequest request,
        CancellationToken cancellationToken)
    {
        var transactionResult = await _transactionManager.BeginTransactionAsync(cancellationToken);

        using var transaction = transactionResult.Value;

        var movingDepartmentResult = await _departmentRepository.GetByIdWithLockAsync(departmentId, cancellationToken);

        if (movingDepartmentResult.IsFailure)
        {
            transaction.Rollback();
            return Error.Failure();
        }

        var movingDepartment = movingDepartmentResult.Value;

        Path oldPath = movingDepartment.Path;

        if (request.ParentId.HasValue)
        {
            var newParentDepartmentResult = await _departmentRepository
                .GetByIdWithLockAsync(request.ParentId.Value, cancellationToken);

            if (newParentDepartmentResult.IsFailure)
            {
                transaction.Rollback();
                return Error.Failure();
            }

            if (newParentDepartmentResult.Value.Id ==movingDepartment.Id)
            {
                return Error.Failure();
            }
            
            var newParentDepartment = newParentDepartmentResult.Value;
            
            var setParentResult = movingDepartmentResult.Value.SetParent(newParentDepartment);

            var result = await _departmentRepository.UpdateSubtreePaths(movingDepartment, oldPath, cancellationToken);
            
            _logger.LogInformation("Установлен новый родитель и обновлен путь и глубина у дочерних подразделений");
            
        }
        
        else
        {
            var setRootResult = movingDepartment.SetParentRoot();
            if (setRootResult.IsFailure)
            {
                transaction.Rollback();
                return setRootResult.Error;
            }
            
            await _departmentRepository.UpdateSubtreePaths(movingDepartment, oldPath, cancellationToken);
        }

        transaction.Commit();

        await _transactionManager.SaveChangesAsync(cancellationToken);

        return movingDepartment.Id;
    }
}