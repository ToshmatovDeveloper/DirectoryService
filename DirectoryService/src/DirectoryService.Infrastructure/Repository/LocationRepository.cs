using CSharpFunctionalExtensions;
using DirectoryService.Application;
using DirectoryService.Application.Location;
using DirectoryService.Domain;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DirectoryService.Infrastructure.Repository;

public class LocationRepository : ILocationRepository
{
    private readonly ApplicationDbContext _dbContext;

    public LocationRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Guid, Error>> AddAsync(Location location, CancellationToken cancellationToken)
    {
        var exist = Exists(location);
        
        if (exist.IsFailure)
        {
            return GeneralErrors.AlreadyExist();
        }
        
        var result = await _dbContext.Locations.AddAsync(location, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return location.Id;
    }
    
    public Result<bool, Error> Exists(Location location)
    {
        var existLocationName = _dbContext.Locations
            .FirstOrDefaultAsync(x => x.Name == location.Name);
        
        var existLocationAddress = _dbContext.Locations
            .FirstOrDefaultAsync(x => x.Address == location.Address);

        if (existLocationName != null || existLocationAddress != null)
        {
            return GeneralErrors.AlreadyExist();
        }

        return true;
    }
}

