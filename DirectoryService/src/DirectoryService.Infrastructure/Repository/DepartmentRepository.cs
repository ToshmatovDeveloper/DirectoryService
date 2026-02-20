using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Department;
using DirectoryService.Contracts.Get;
using DirectoryService.Domain;
using DirectoryService.Infrastructure.Factory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;
using Path = DirectoryService.Domain.ValueObjects.Path;

namespace DirectoryService.Infrastructure.Repository;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ILogger<DepartmentRepository> _logger;

    public DepartmentRepository(ApplicationDbContext dbContext, ILogger<DepartmentRepository> logger, ISqlConnectionFactory sqlConnectionFactory)
    {
        _dbContext = dbContext;
        _logger = logger;
        _sqlConnectionFactory = sqlConnectionFactory;
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

    public async Task<Result<Department, Error>> GetByIdWithLockAsync(Guid? id, CancellationToken cancellationToken)
    {
        try
        {
            var department = await _dbContext.Departments
                .FromSql($"SELECT * FROM departments WHERE Id = {id} FOR UPDATE ")
                .FirstOrDefaultAsync(cancellationToken);

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
            await _dbContext.Database.ExecuteSqlRawAsync(
                "SELECT capacity FROM department_id WHERE Id = @depatmentId FOR UPDATE ", depatmentId,  cancellationToken);
            
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

    public async Task<UnitResult<Error>> UpdateSubtreePaths(Department department, Path oldPath,
        CancellationToken cancellationToken)
    {
        using var connection = await _sqlConnectionFactory.CreateConnectionAsync(cancellationToken);

        const string sql = """
                           UPDATE departments
                           SET depth = @departmentDepth + (depth - nlevel(@oldPath::ltree) + 1),
                               path = @departmentPath::ltree || subpath(path, nlevel(@oldPath::ltree))
                           WHERE path <@ @oldPath::ltree
                           AND path != @oldPath::ltree
                           """;

        return UnitResult.Success<Error>();
    }

    public async Task<IReadOnlyList<DepartmentDto>> GetRootDepartment(int page, int size, int prefetch, CancellationToken cancellationToken)
    {
        var skip = (page - 1) * size;
        
        var query = _dbContext.Departments
            .Where(d => d.ParentId == null)
            .OrderBy(d => d.Name)
            .Skip(skip)
            .Take(size)
            .Select(root => new DepartmentDto
            {
                Id = root.Id,
                Name = root.Name.Value,
                Children = root.Children
                    .OrderBy(c => c.Name)
                    .Take(prefetch)
                    .Select(child => new DepartmentDto
                    {
                        Id = child.Id,
                        Name = child.Name.Value,
                        HasMoreChildren = _dbContext.Departments
                            .Any(x => x.ParentId == child.Id)
                    }).ToList(),
                HasMoreChildren = _dbContext.Departments
                    .Any(x => x.ParentId == root.Id)
            });
        
        return await query.ToListAsync(cancellationToken);
    }

    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}