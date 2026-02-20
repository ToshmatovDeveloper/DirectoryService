using DirectoryService.Application.Abstractions;
using DirectoryService.Application.GetRequests;
using DirectoryService.Contracts.Get;
using Shared;

namespace DirectoryService.Application.Department.Query;

public class GetDepartmentsHandler
{
    private readonly IDepartmentRepository _departmentRepository;

    public GetDepartmentsHandler(IReadDbContext context, IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<PaginationResponse<DepartmentDto>> Handle(
        GetDepartmentsRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _departmentRepository.GetRootDepartment(
            request.Page,
            request.Size,
            request.Prefetch,
            cancellationToken);

        var totalCount = result.Count();
        
        return new PaginationResponse<DepartmentDto>(result, totalCount, result.Count());

    }
}