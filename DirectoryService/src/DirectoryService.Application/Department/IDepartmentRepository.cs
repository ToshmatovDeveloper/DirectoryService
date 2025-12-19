using CSharpFunctionalExtensions;
using DirectoryService.Domain.ValueObjects;
using Shared;

namespace DirectoryService.Application.Department;

public interface IDepartmentRepository
{
    Task<Result<DepartmentId,Error>> AddAsync(Domain.Department department, CancellationToken cancellationToken);
    
    Task<Result<Domain.Department, Error>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
}