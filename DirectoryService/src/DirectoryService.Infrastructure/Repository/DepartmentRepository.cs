using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Department;
using DirectoryService.Contracts.Get;
using DirectoryService.Domain;
using DirectoryService.Infrastructure.Factory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
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

    public async Task<UnitResult<Error>> LockChildrenByPath(
        string oldPath,
        CancellationToken cancellationToken)
    {
        await using var transaction = await _dbContext.Database
            .BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancellationToken);

        try
        {
            await _dbContext.Database.ExecuteSqlRawAsync(
                @"
            SELECT 1
            FROM departments
            WHERE path <@ @oldPath::ltree
              AND is_deleted = false
            FOR UPDATE NOWAIT
            ",
                new NpgsqlParameter("oldPath", oldPath));

            return UnitResult.Success<Error>();
        }
        catch (PostgresException pgEx) when (pgEx.SqlState == PostgresErrorCodes.LockNotAvailable)
        {
            _logger.LogWarning(
                "Descendants already locked for path: {Path}",
                oldPath);

            return Error.Failure(new ErrorMessage("", "", ""));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error locking descendants by path: {Path}",
                oldPath);

            return Error.Failure(new ErrorMessage("", "", ""));
        }
    }

    public async Task<UnitResult<Error>> MarkDepartmentAsDeleted(string prefix, Guid deletedDepartmentId, CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@prefix", prefix);
        parameters.Add("@deleted_department_id", deletedDepartmentId);

        try
        {
            const string sql =
                """
                UPDATE departments dept
                SET 
                    path =  subpath(dept.path, 0, -1) || (@prefix|| subpath(dept.path, -1)::text)::ltree
                    WHERE dept.is_deleted = true
                        AND dept.id = @deleted_department_id
                """;

            var connection = _dbContext.Database.GetDbConnection();
            await connection.ExecuteAsync(sql, parameters);

            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Path of department: {deletedDepartmentId} was updated",
                deletedDepartmentId);
            return Error.Failure(new ErrorMessage("", "", ""));
        }
    }

    public async Task<UnitResult<Error>> UpdateAllDescendantsPath(string oldPath, string newPath, Guid parentDepartmentId,
        CancellationToken cancellationToken)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@oldPath", oldPath);
        parameters.Add("@newPath", newPath);
        parameters.Add("@parent_department_id", parentDepartmentId);
        parameters.Add("@updated_at", DateTime.UtcNow);

        try
        {
            await _dbContext.Database.ExecuteSqlRawAsync(
                $"""
                 UPDATE departments dept
                 SET 
                     path = @newPath::ltree || subpath(dept.path, nlevel(@oldPath::ltree)),
                     depth = nlevel(@newPath::ltree) + (dept.depth - nlevel(@oldPath::ltree)),
                     updated_at = @updated_at
                 WHERE dept.is_deleted = false
                         AND dept.path <@ @oldPath::ltree
                         AND dept.id != @parent_department_id
                 """,
                parameters);

            return UnitResult.Success<Error>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Update error for descendants of department{parentDepartmentId}", parentDepartmentId);
            return Error.Failure(new ErrorMessage("", "", ""));
        }    }

    public async Task<Result<Department, Error>> GetWithChildrenAsync(Guid departmentId, CancellationToken cancellationToken)
    {
        var department = await _dbContext.Departments
            .Include(d => d.Children)
            .FirstOrDefaultAsync(d => d.Id == departmentId, cancellationToken);

        if (department is null)
            return Result.Failure<Department, Error>(GeneralErrors.NotFound());

        return department;
    }


    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}