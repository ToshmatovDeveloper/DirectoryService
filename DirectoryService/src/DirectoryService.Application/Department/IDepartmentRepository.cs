using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Application.Department;

public interface IDepartmentRepository
{
    Task<Result<Guid,Error>> AddAsync(Domain.Department department, CancellationToken cancellationToken);
    
}