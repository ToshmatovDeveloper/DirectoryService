using DirectoryService.Domain.ValueObjects;
using TimeZone = DirectoryService.Domain.ValueObjects.TimeZone;

namespace DirectoryService.Domain;

public class Location
{
    public Location()
    {
        
    }

    private Location(Guid id, 
                     Name name, 
                     Address address,
                     TimeZone timeZone,
                     bool isActive,
                     DateTime createdAt,
                     DateTime updatedAt)
    {
        Id = id;
        Name = name;
        Address = address;
        TimeZone = timeZone;
        IsActive = isActive;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
        
    public Guid Id { get; set; }
    
    public Name Name { get; set; }
    
    public Address Address { get; set; }
    
    public TimeZone TimeZone { get; set; }

    public bool IsActive { get; private set; } = true;
    
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
    
    public List<Department> DepartmentId { get; private set; }

    public List<DepartmentLocation> Departments { get; private set; }

}