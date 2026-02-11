namespace DirectoryService.Contracts.GetRequests;

public record GetLocationRequest(  
    string? Search,
    bool  IsActive,
    Guid? DepartmentId,
    int Page,
    int PageSize,
    string SortBy = "date",
    string SortDirection = "asc");