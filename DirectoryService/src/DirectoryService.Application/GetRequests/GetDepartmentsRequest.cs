namespace DirectoryService.Application.GetRequests;

public record GetDepartmentsRequest(  
    string? Search,
    bool  IsActive,
    Guid? DepartmentId,
    int Page = 1,
    int PageSize = 5,
    string SortBy = "positions",
    string SortDirection = "asc");