using System.Text.RegularExpressions;

namespace DirectoryService.Domain;

public class Department
{
    private Department()
    {
        
    }
    
    private Department(Name name, Identifier identifier, Path path)
    {
        if (name == null || 3 > name.ToString().Length || name.ToString().Length <150)
        {
            throw new Exception("Wrong name");
        }
        Name = name;

        if (name == null || 3 > name.ToString().Length || name.ToString().Length <150 || !Regex.IsMatch(identifier.ToString(), @"^[a-zA-Z0-9]*$"))
        {
            throw new Exception("Wrong identifier");
        }
        Identifier = identifier;
        
        Path = path;
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
