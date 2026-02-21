using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.GetRequests;
using DirectoryService.Contracts.Get;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace DirectoryService.Application.Department.Query;

public class GetTopDepartmentsHandler
{
    private readonly IReadDbContext _context;

    public GetTopDepartmentsHandler(IReadDbContext context)
    {
        _context = context;
    }

    public async Task<PaginationResponse<DepartmentDto>> Handle(
        GetTopDepartmentsRequest request,
        CancellationToken cancellationToken)
    {
        var departmentQuery = _context.DepartmentsRead.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.ToLower();
            departmentQuery = departmentQuery.Where(l =>
                EF.Functions.Like(l.Name.Value.ToLower(), $"%{search}%"));
        }
        
        Expression<Func<Domain.Department, object>> keySelector =
            request.SortBy?.ToLower() switch
            {
                "name" => d => d.Name.Value,
                "date" => d => d.CreatedAt,
                _ => d => d.Positions.Count
            };
        
        departmentQuery = request.SortDirection?.ToLower() == "asc"
            ? departmentQuery.OrderBy(keySelector)
            : departmentQuery.OrderByDescending(keySelector);
        
        var items = await departmentQuery
            .Skip((request.Page - 1) * request.PageSize)
            .Take(5)
            .Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name.Value,
                Identifier = d.Identifier.Value,
                IsActive = d.IsActive,
                ParentId = d.ParentId,
                Path = d.Path.Value,
                PositionsCount = d.Positions.Count
            })
            .ToListAsync(cancellationToken);

        var totalCount = await departmentQuery.CountAsync(cancellationToken);

        return new PaginationResponse<DepartmentDto>(
            items,
            request.Page,
            totalCount);
    }
}