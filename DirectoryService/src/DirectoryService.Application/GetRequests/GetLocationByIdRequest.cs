using DirectoryService.Application.Abstractions;

namespace DirectoryService.Contracts.GetRequests;

public class GetLocationByIdRequest 
{
    public GetLocationByIdRequest(Guid locationId)
    {
        LocationId = locationId;
    }
    public Guid LocationId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
