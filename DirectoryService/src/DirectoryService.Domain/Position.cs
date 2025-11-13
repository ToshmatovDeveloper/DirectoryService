using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.Domain;

public class Position
{
    private Position(Guid id, 
                     Name name, 
                     string description, 
                     bool isActive,
                     DateTime createdAt,
                     DateTime updatedAt)
    {
        Id=id;
        Name=name;
        Description=description;
        IsActive=isActive;
        CreatedAt=createdAt;
        UpdatedAt=updatedAt;
    }
    
    public Guid Id { get; private set; }
    
    public Name Name { get; private set; }
    
    public string Description { get; private set; }
    
    public bool IsActive { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }

    public List<DepartmentPosition> Departments { get; private set; }
}