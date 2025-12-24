namespace DirectoryService.Domain.ValueObjects;

public record LocationId
{
    public Guid Value { get; }
    
    public LocationId(Guid locationId)
    {
        Value = locationId;
    }

    public static LocationId Create(Guid locationId) => new LocationId(locationId);
}

