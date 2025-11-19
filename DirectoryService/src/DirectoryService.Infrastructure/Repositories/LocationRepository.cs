using DirectoryService.Application.Location;
using DirectoryService.Contracts.Location;
using DirectoryService.Domain;

namespace DirectoryService.Infrastructure.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly ApplicationDbContext _dbContext;

    public LocationRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> AddAsync(Location location, CancellationToken cancellationToken)
    {
        await _dbContext.Locations.AddAsync(location, cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return location.Id;
    }

    
}