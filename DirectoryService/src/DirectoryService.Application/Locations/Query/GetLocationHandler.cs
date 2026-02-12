using System.Linq.Expressions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts.Get;
using DirectoryService.Contracts.GetRequests;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DirectoryService.Application.Location.Query;

public class GetLocationHandler
{
    private readonly IReadDbContext _context;

    public GetLocationHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<PaginationResponse<LocationDto>> Handle(
        GetLocationRequest request,
        CancellationToken cancellationToken)
    {
        var locationsQuery = _context.LocationsRead.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            locationsQuery = locationsQuery.Where(l =>
                EF.Functions.Like(l.Name.Value.ToLower(), $"%{search}%"));
        }
        
        if (request.DepartmentId.HasValue)
        {
            locationsQuery = locationsQuery.Where(l =>
                l.DepartmentId.Any(d => d.Id == request.DepartmentId));
        }

        Expression<Func<Domain.Location, object>> keySelector =
            request.SortBy?.ToLower() switch
            {
                "name" => l => l.Name.Value,
                "date" => l => l.CreatedAt,
                _ => l => l.CreatedAt
            };

        locationsQuery = request.SortDirection?.ToLower() == "asc"
            ? locationsQuery.OrderBy(keySelector)
            : locationsQuery.OrderByDescending(keySelector);

        var totalCount = await locationsQuery.CountAsync(cancellationToken);

        var items = await locationsQuery
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
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

        return new PaginationResponse<LocationDto>(items, request.Page, totalCount);
    }
}
