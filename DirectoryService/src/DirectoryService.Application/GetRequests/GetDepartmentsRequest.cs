namespace DirectoryService.Application.GetRequests;

public record GetDepartmentsRequest(int Page, int Size, int Prefetch);