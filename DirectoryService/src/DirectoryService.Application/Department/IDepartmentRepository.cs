using CSharpFunctionalExtensions;
using DirectoryService.Application.Department.Update;
using DirectoryService.Contracts.Update;
using DirectoryService.Domain.ValueObjects;
using Shared;
using Path = DirectoryService.Domain.ValueObjects.Path;
using Department = DirectoryService.Domain.Department;

namespace DirectoryService.Application.Department;

public interface IDepartmentRepository
{
    Task<Result<Guid,Error>> AddAsync(Domain.Department department, CancellationToken cancellationToken);
    
    Task<Result<Domain.Department, Error>> GetByIdWithLockAsync(Guid? id, CancellationToken cancellationToken);
    
    Task<Result<Domain.Department, Error>> GetByIdWithLocationAsync(Guid depatmentId, 
        CancellationToken cancellationToken);
    
    Task<UnitResult<Error>> UpdateSubtreePaths(
        Domain.Department department,
        Path oldPath,
        CancellationToken cancellationToken);

    
}