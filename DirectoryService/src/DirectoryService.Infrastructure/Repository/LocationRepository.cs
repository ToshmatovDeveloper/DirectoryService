using CSharpFunctionalExtensions;
using Dapper;
using DirectoryService.Application;
using DirectoryService.Application.Location;
using DirectoryService.Application.Locations;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace DirectoryService.Infrastructure.Repository;

public class LocationRepository : ILocationRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<LocationRepository> _logger;

    public LocationRepository(ApplicationDbContext dbContext, ILogger<LocationRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> AddAsync(Location location, CancellationToken cancellationToken)
    {
        var existsByName = await ExistsByName(location);
        
        var existsByAddress = await ExistsByAddress(location);
        
        if (existsByName.IsFailure || existsByAddress.IsFailure)
        {
            return GeneralErrors.AlreadyExist();
        }

        try
        {
            var result = await _dbContext.Locations.AddAsync(location, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
            return Error.Failure();        
        }
    
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return location.Id;
    }
    
    public async Task<Result<bool, Error>> ExistsByName(Location location)
    {
        var existLocationName = await _dbContext.Locations
            .FirstOrDefaultAsync(x => x.Name == location.Name);
        
        if (existLocationName != null)
            return GeneralErrors.AlreadyExist();
        
        return true;
    }
    
    public async Task<Result<bool, Error>> ExistsByAddress(Location location)
    {
        var existLocationAddress = await _dbContext.Locations
            .FirstOrDefaultAsync(x => x.Address == location.Address);
        
        if (existLocationAddress != null)
            return GeneralErrors.AlreadyExist();
        

        return true;
    }

    public async Task<Result<Error>> CheckActiveLocationsDyId(
        IEnumerable<LocationId> locationIds, CancellationToken cancellationToken)
    {
        var ids = locationIds.Select(x => x.Value).ToList();

        var result = await _dbContext.Locations
            .AnyAsync(
                x => ids.Contains(x.Id) && x.IsActive,
                cancellationToken);

        if (result)
        {
            return Error.Failure();
        }
        
        return GeneralErrors.AlreadyExist();
    }

    public async Task<Location?> GetById(Guid locationId, CancellationToken cancellationToken)
    {
        return await _dbContext.Locations
            .Include(x => x.Address)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == locationId, cancellationToken);
        
    }

    public async Task<UnitResult<Error>> SoftDeleteUniqDepRelatedLocations(Guid departmentId, CancellationToken cancellationToken = default)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@department_id", departmentId);
        parameters.Add("@deleted_at", DateTime.UtcNow);
        parameters.Add("@updated_at", DateTime.UtcNow);

        try
        {
            const string sql =
                """
                    WITH unique_locations AS (
                    SELECT dl1.location_id
                    FROM department_locations dl1
                    WHERE dl1.department_id = @department_id
                      AND NOT EXISTS (
                        SELECT 1
                        FROM department_locations dl2
                        WHERE dl2.location_id = dl1.location_id
                          AND dl2.department_id != @department_id
                    )
                )
                UPDATE locations l
                SET is_deleted = true,
                    deleted_at = @deleted_at,
                    updated_at = @updated_at
                FROM unique_locations 
                WHERE l.id = unique_locations.location_id AND l.is_deleted = false;
                """;

            var connection = _dbContext.Database.GetDbConnection();
            var updatedLocations = await connection.ExecuteAsync(sql, parameters);

            _logger.LogInformation("Count of updated locations: {updatedLocations}", updatedLocations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Update error for locations of department{departmentId}", departmentId);
            return Error.Failure(new ErrorMessage("", "", ""));
        }

        return UnitResult.Success<Error>();
    }
}

