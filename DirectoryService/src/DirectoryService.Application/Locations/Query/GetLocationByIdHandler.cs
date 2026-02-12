using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts.Get;
using DirectoryService.Contracts.GetRequests;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DirectoryService.Application.Location.Query;

public class GetLocationByIdHandler 
{
    private readonly IReadDbContext _readDbContext;

    public GetLocationByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }
    public async Task<GetLocationByIdDto?> Handle(GetLocationByIdRequest request, CancellationToken cancellationToken)
    {
        var location = await _readDbContext.LocationsRead
            .Where(l => l.Id == request.LocationId)
            .Select(l => new
            {
                l.Id,
                Name = l.Name.Value,
                Country = l.Address.Country,
                City = l.Address.City,
                Street = l.Address.Street,
                TimeZone = l.TimeZone.Value,
                l.IsActive,
                l.CreatedAt,
                l.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (location is null)
            return null;

        var departmentsQuery = _readDbContext.DepartmentsRead
            .Where(d => d.LocationId.Any(loc => loc.Id == request.LocationId));

        var totalCount = await departmentsQuery.CountAsync(cancellationToken);

        return new GetLocationByIdDto
        {
            Id = location.Id,
            Name = location.Name,
            Country = location.Country,
            City = location.City,
            Street = location.Street,
            TimeZone = location.TimeZone,
            IsActive = location.IsActive,
            CreatedAt = location.CreatedAt,
            UpdatedAt = location.UpdatedAt,
        };
    }
    
}