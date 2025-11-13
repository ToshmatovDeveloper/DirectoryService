using DirectoryService.Domain.ValueObjects;
using System.Text.RegularExpressions;
using Path = DirectoryService.Domain.ValueObjects.Path;

namespace DirectoryService.Domain;

public class Department
{
    private readonly Guid _id;
    private readonly Name _name;
    private readonly Identifier _identifier;
    private readonly Guid _parentId;
    private readonly Path _path;
    private readonly short _depth;
    private readonly bool _isActive;
    private readonly DateTime _createdAt;
    private readonly DateTime _updatedAt;
    private readonly Department _parent;

    private Department()
    {
        
    }
    
    private Department(Guid id, 
                       Name name, 
                       Identifier identifier, 
                       Guid parentId, 
                       Path path,
                       short depth,
                       bool isActive,
                       DateTime createdAt,
                       DateTime updatedAt,
                       Department parent)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
        ParentId = parentId;
        Path = path;
        Depth = depth;
        IsActive = isActive;
        CreatedAt = createdAt;
        Updated = updatedAt;
        Parent = parent;
    }
    
    public Guid Id { get; private set; }
    
    public Name Name { get; private set; }
    
    public Identifier Identifier { get; private set; }
    
    public Guid ParentId { get; private set; }
    
    public Path Path { get; private set; }
    
    public short Depth { get; private set; }
    
    public bool IsActive { get; private set; } = true;
    
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    
    public DateTime Updated { get; private set; } = DateTime.UtcNow;

    public Department? Parent { get; private set; }
    
    public List<Department> Children { get; private set; }
    
    public List<Guid>? PositionsId { get; private set; }
    
    public List<Location> LocationId { get; private set; }
    
    /// <summary>
    /// Added list of departmentPosition and departmentLocation
    /// </summary>
    public List<DepartmentPosition> Positions { get; private set; }
    
    public List<DepartmentLocation> Locations { get; private set; }

}