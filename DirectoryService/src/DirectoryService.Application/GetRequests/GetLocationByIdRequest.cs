using DirectoryService.Application.Abstractions;

namespace DirectoryService.Contracts.GetRequests;

public record GetLocationByIdRequest(Guid LocationId, int Page, int PageSize);
