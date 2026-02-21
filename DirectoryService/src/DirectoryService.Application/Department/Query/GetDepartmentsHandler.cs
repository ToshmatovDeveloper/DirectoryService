using DirectoryService.Application.Abstractions;
using DirectoryService.Application.GetRequests;
using DirectoryService.Contracts.Get;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DirectoryService.Application.Department.Query;

public class GetDepartmentsHandler
{
    private readonly IReadDbContext _readDbContext;
    
    public GetDepartmentsHandler(IReadDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<PaginationResponse<DepartmentDto>> Handle(
        GetDepartmentsRequest request,
        CancellationToken cancellationToken)
    {
        var skip = (request.Page - 1) * request.Size;
        
        var query = _readDbContext.DepartmentsRead
            .AsQueryable()
            .Where(d => d.ParentId == null)
            .OrderBy(d => d.Name)
            .Skip(skip)
            .Select(r => new DepartmentDto
            {
                Id = r.Id,
                Name = r.Name.Value,
                Children = r.Children
                    .OrderBy(c => c.Name)
                    .Take(request.Prefetch)
                    .Select(child => new DepartmentDto
                    {
                        Id = child.Id,
                        Name = child.Name.Value,
                        HasMoreChildren = _readDbContext.DepartmentsRead
                            .Any(c => c.ParentId == child.Id)
                    }).ToList(),
                HasMoreChildren = _readDbContext.DepartmentsRead
                    .Any(d => d.ParentId == r.Id)
            });
        var totalCount = query.Count();
        
        var items = await query.ToListAsync(cancellationToken);
        
        return new PaginationResponse<DepartmentDto>(items, totalCount, query.Count());

    }
}