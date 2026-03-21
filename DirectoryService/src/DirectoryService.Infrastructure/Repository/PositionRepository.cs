using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application.Position;
using DirectoryService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace DirectoryService.Infrastructure.Repository;

public class PositionRepository : IPositionRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<PositionRepository> _logger;

    public PositionRepository(ApplicationDbContext dbContext, ILogger<PositionRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> AddAsync(Position position, CancellationToken cancellationToken)
    {
        var result = await _dbContext.Positions.AddAsync(position, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return position.Id;
    }
    
    public async Task<Result<bool, Error>> AlreadyExistPosition(Position position, CancellationToken cancellationToken)
    {
        var existPositionName = await _dbContext.Positions
            .FirstOrDefaultAsync(x => x.Name == position.Name);

        if (existPositionName == null)
            return false;
        
        return true;
    }

    public async Task<UnitResult<Error>> SoftDeleteUniqDepRelatedPositions(Guid departmentId, CancellationToken cancellationToken = default)
    {
        var parameters = new DynamicParameters();

        parameters.Add("@department_id", departmentId);
        parameters.Add("@deleted_at", DateTime.UtcNow);
        parameters.Add("@updated_at", DateTime.UtcNow);

        try
        {
            const string sql =
                """
                    WITH unique_positions AS (
                    SELECT dp1.position_id
                    FROM department_positions dp1
                    WHERE dp1.department_id = @department_id
                      AND NOT EXISTS (
                        SELECT 1
                        FROM department_positions dp2
                        WHERE dp2.position_id = dp1.position_id
                          AND dp2.department_id != @department_id
                    )
                )
                UPDATE positions p
                SET is_deleted = true,
                    deleted_at = @deleted_at,
                    updated_at = @updated_at
                FROM unique_positions 
                WHERE p.id = unique_positions.position_id AND p.is_deleted = false;
                """;

            var connection = _dbContext.Database.GetDbConnection();
            int updatedPositions = await connection.ExecuteAsync(sql, parameters);

            _logger.LogInformation("Count of updated positions: {updatedPostions}", updatedPositions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Update error for positions of department{departmentId}", departmentId);
            return Error.Failure(new ErrorMessage("", "", ""));
        }

        return UnitResult.Success<Error>();
    }
}