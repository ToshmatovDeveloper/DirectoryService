using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Locations;
using Shared;

namespace DirectoryService.Application.Department.Delete;

public class SoftDeleteHandler
{
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ITransactionManager _transactionManager;

    public SoftDeleteHandler(IDepartmentRepository departmentRepository, ITransactionManager transactionManager)
    {
        _departmentRepository = departmentRepository;
        _transactionManager = transactionManager;
    }

    public async Task<Result<Domain.Department, Error>> Handle(Guid id, CancellationToken cancellationToken)
    {
        var transactionResult = await _transactionManager.BeginTransactionAsync(cancellationToken);
        
        using var transaction = transactionResult.Value;

        var departmentResult = await _departmentRepository.SoftDeleteAsync(id, cancellationToken);

        if (departmentResult.IsFailure)
        {
            transaction.Rollback();
            return Error.Failure();
        }
        
        var department = departmentResult.Value;
        
        if (department.LocationsDepartmentCounter())
        {
            department.Locations.First(l => l.Location.SoftDelete());
        }
    
        if (department.PositionsDepartmentCounter())
            department.Positions.First(p => p.Position.SoftDelete());

        var oldPath = department.Path;
        
        oldPath.UpdatePath(oldPath);
        
        department.Path.UpdatePath(oldPath);
        
        await _transactionManager.SaveChangesAsync(cancellationToken);
        
        return departmentResult;
    }
}