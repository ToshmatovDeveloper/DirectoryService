namespace DirectoryService.Contracts.Get;

public record LocationDto
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = string.Empty;
    
    public string Country { get; init; } = string.Empty;
    
    public string City { get; init; } = string.Empty;
    
    public string Street { get; init; } = string.Empty;
    
    public string TimeZone { get; init; } = string.Empty;
    
    public bool IsActive { get; init; }
    
    public DateTime CreatedAt { get; init; }
    
    public DateTime UpdatedAt { get; init; }
}