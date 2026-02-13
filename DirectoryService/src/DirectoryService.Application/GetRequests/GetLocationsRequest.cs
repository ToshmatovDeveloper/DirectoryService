namespace DirectoryService.Application.GetRequests;

public record GetLocationsRequest(  
    string? Search,
    bool  IsActive,
    Guid? DepartmentId,
    int Page,
    int PageSize,
    string SortBy = "date",
    string SortDirection = "asc");