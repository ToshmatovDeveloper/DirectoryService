using CSharpFunctionalExtensions;
using DirectoryService.Application.Department;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DirectoryService.Infrastructure.Repository;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DepartmentRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<DepartmentId, Error>> AddAsync(Department department, CancellationToken cancellationToken)
    {
        var addDepartmentResult = await _dbContext.Departments.AddAsync(department, cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return department.Id;

    }

    public async Task<Result<Department, Error>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var department = await _dbContext.Departments
                .Include(d => d.Parent)
                .FirstOrDefaultAsync(d => d.Id.Value == id, cancellationToken);

            if (department == null)
            {
                return Result.Failure<Department, Error>(
                    GeneralErrors.NotFound());
            }

            return Result.Success<Department, Error>(department);
        }
        catch (Exception ex)
        {
            return Result.Failure<Department, Error>(
                GeneralErrors.Failure());
        }
    }
}