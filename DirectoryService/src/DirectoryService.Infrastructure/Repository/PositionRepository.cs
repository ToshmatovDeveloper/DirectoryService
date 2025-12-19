using CSharpFunctionalExtensions;
using DirectoryService.Application.Position;
using DirectoryService.Domain;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DirectoryService.Infrastructure.Repository;

public class PositionRepository : IPositionRepository
{
    private readonly ApplicationDbContext _dbContext;

    public PositionRepository(ApplicationDbContext context, ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
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
}