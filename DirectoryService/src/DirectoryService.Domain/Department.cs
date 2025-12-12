using System.Text.RegularExpressions;
using DirectoryService.Domain.ValueObjects;
using Path = DirectoryService.Domain.ValueObjects.Path;

namespace DirectoryService.Domain;

public class Department
{
    public Department(Guid id,
        Name name,
        Identifier identifier,
        Guid? parentId,
        Guid[] locationIds)
    {
        Id = id;
        Name = name;
        Identifier = identifier;
        ParentId = parentId;
        LocationId = locationIds;
    }
    
    public Guid Id { get; private set; }
    
    public Name Name { get; private set; }
    
    public Identifier Identifier { get; private set; }
    
    public Guid? ParentId { get; private set; }
    
    public Path Path { get; private set; }
    
    public short Depth { get; private set; }
    
    public bool IsActive { get; private set; } = true;
    
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    
    public DateTime Updated { get; private set; } = DateTime.UtcNow;

    public Department? Parent { get; private set; }
    
    public List<Department> Children { get; private set; }
    
    
    public List<Guid>? PositionsId { get; private set; }
    
    public Guid[] LocationId { get; private set; }
    
    /// <summary>
    /// Added list of departmentPosition and departmentLocation
    /// </summary>
    public List<DepartmentPosition> Positions { get; private set; }
    
    public List<DepartmentLocation> Locations { get; private set; }

}
