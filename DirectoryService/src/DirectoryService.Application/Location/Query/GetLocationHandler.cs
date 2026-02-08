using System.Linq.Expressions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts.Get;
using DirectoryService.Contracts.GetRequests;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Application.Location.Query;

public class GetLocationHandler
{
    private readonly IReadDbContext  _context;
    
    public GetLocationHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<GetLocationsDto> Handle(GetLocationRequest request, CancellationToken cancellationToken)
    {
        var locationsQuery = _context.LocationsRead;

        if(!string.IsNullOrWhiteSpace(request.Search))
            locationsQuery = locationsQuery.Where(l => 
                EF.Functions.Like(l.Name.Value.ToLower(), $"%{request.Search.ToLower()}%"));
        
        if (request.DepartmentId.HasValue)
            locationsQuery = locationsQuery.Where(l => l.DepartmentId.Any(loc => loc.Id == request.DepartmentId));

        Expression<Func<Domain.Location, object>> keySelector = request.SortBy?.ToLower() switch
        {
            "name" => l => l.Name,
            "date" => l => l.CreatedAt,
            _ => l => l.CreatedAt
        };
        
        locationsQuery = locationsQuery
            .OrderBy(keySelector);
        
        locationsQuery = request.SortDirection == "asc"
            ? locationsQuery.OrderBy(keySelector)
            : locationsQuery.OrderByDescending(keySelector);
        
        var totalCount = await locationsQuery.LongCountAsync(cancellationToken);
        
        locationsQuery = locationsQuery
            .Skip((request.Page - 1)*request.PageSize)
            .Take(request.PageSize);
        
        var locations = await locationsQuery
            .Select(l => new LocationDto
            {  
                Id = l.Id,
                Name = l.Name.Value,
                Country = l.Address.Country,
                City = l.Address.City,
                Street = l.Address.Street,
                TimeZone = l.TimeZone.Value,
                IsActive = l.IsActive,
                CreatedAt = l.CreatedAt,
                UpdatedAt = l.UpdatedAt,
                
            })
            .ToListAsync(cancellationToken);

        return new GetLocationsDto(locations, totalCount);
    }
}