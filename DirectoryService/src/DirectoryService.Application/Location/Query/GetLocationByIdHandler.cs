using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts.Get;
using DirectoryService.Contracts.GetRequests;
using Microsoft.EntityFrameworkCore;

namespace DirectoryService.Application.Location.Query;

public class GetLocationByIdHandler
{
    public readonly IReadDbContext _readDbContext;

    public GetLocationByIdHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<GetLocationByIdDto?> Handle(GetLocationByIdRequest request, CancellationToken cancellationToken)
    {
        return await _readDbContext.LocationsRead
            .Include(l => l.Departments)
            .Where(l => l.Id == request.LocationId)
            .Select(l => new GetLocationByIdDto
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
                Departmens = _readDbContext.DepartmentsRead
                    .Where(d => d.LocationId.Any(loc => loc.Id == l.Id))
                    .Select(d => new DepartmentDto
                    {
                        Id = d.Id,
                        Identifier = d.Identifier.Value,
                        Name = d.Name.Value,
                        ParentId = d.ParentId!.Value,
                        Path = d.Path.Value,
                        IsActive = _readDbContext.DepartmentsRead
                            .Any(ad => ad.Id == d.Id
                                       && ad.LocationId.Any(loc => loc.Id == l.Id))
                    }).ToList()
            }).FirstOrDefaultAsync(cancellationToken);
    }
}