using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.Domain;

public record Position
{
    private Position()
    {
        
    }
    private Position(Name name, string description)
    { 
        Name = name;
        Description = description;
    }
    
    public Guid Id { get; private set; }
    
    public Name Name { get; private set; }
    
    public string Description { get; private set; }
    
    public bool IsActive { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
    
    public List<DepartmentPosition> Departments { get; private set; }
}