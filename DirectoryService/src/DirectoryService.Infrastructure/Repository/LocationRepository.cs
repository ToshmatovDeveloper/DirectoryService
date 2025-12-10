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
        var existsByName = await ExistsByName(location);
        
        var existsByAddress = await ExistsByAddress(location);
        
        if (existsByName.IsFailure || existsByAddress.IsFailure)
        {
            return GeneralErrors.AlreadyExist();
        }
        
        var result = await _dbContext.Locations.AddAsync(location, cancellationToken);

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
}

