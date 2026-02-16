namespace DirectoryService.Contracts.Get;

public record DepartmentDto
{
    
    public Guid Id { get; init; }

    public string Name { get; init; } 

    public string Identifier { get; init; } 

    public string Path { get; init; }

    public Guid? ParentId { get; init; }
    
    public bool IsActive { get; init; }
    
    public int PositionsCount { get; init; }
    
}