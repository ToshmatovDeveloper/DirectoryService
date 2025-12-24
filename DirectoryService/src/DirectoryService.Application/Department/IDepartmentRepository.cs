using CSharpFunctionalExtensions;
using DirectoryService.Contracts.Update;
using DirectoryService.Domain.ValueObjects;
using Shared;

namespace DirectoryService.Application.Department;

public interface IDepartmentRepository
{
    Task<Result<Guid,Error>> AddAsync(Domain.Department department, CancellationToken cancellationToken);
    
    Task<Result<Domain.Department, Error>> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<Result<Domain.Department, Error>> GetByIdWithLocationAsync(Guid depatmentId, CancellationToken cancellationToken);
    
}