using CSharpFunctionalExtensions;
using DirectoryService.Application.Department;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace DirectoryService.Infrastructure.Repository;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<DepartmentRepository> _logger;

    public DepartmentRepository(ApplicationDbContext dbContext, ILogger<DepartmentRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> AddAsync(Department department, CancellationToken cancellationToken)
    {
        try
        {
            var addDepartmentResult = await _dbContext.Departments.AddAsync(department, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Error.Failure();
        }
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return department.Id;
    }

    public async Task<Result<Department, Error>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var department = await _dbContext.Departments
                .Include(d => d.Parent)
                .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

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

    public async Task<Result<Department, Error>> GetByIdWithLocationAsync(Guid depatmentId, CancellationToken cancellationToken)
    {   
        try
        {
            var department = await _dbContext.Departments
                .Include(d => d.Locations)
                .FirstOrDefaultAsync(d => d.Id == depatmentId, cancellationToken);

            if (department == null)
            {
                return Error.NotFound();
            }

            return department;
        }
        catch (Exception ex)
        {
            return Result.Failure<Department, Error>(
                GeneralErrors.Failure());
        }
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}