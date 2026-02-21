namespace DirectoryService.Contracts.Get;

public record DepartmentDto
{
    
    public Guid Id { get; init; }

    public string Name { get; init; } 

    public string Identifier { get; init; } 

    public string Path { get; init; }

    public Guid? ParentId { get; init; }
    
    public bool IsActive { get; init; }
    
    public int Depth { get; init; }
    
    public DateTime CreateAt {get; init;}
    
    public DateTime UpdatedAt {get; init;}
    
    public int PositionsCount {get; init;}
    
    public List<DepartmentDto> Children { get; init; }
    
    public bool HasMoreChildren {get; init;}

}