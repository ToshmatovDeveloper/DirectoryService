using DirectoryService.Domain.ValueObjects;
using TimeZone = DirectoryService.Domain.ValueObjects.TimeZone;

namespace DirectoryService.Domain;

public record Location
{
    private Location()
    {
        
    }

    public Location(Guid id,
                     Name name,
                     Address address,
                     TimeZone timeZone)
    {
        Id = id;
        Name = name;
        Address = address;
        TimeZone = timeZone;
    }

    public Guid Id { get; private set; }
    
    public Name Name { get; private set; }
    
    public Address Address { get; private set; }
    
    public TimeZone TimeZone { get; private set; }
    
    public bool IsActive { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
    
    public List<Department> DepartmentId { get; private set; } 
    
    public List<DepartmentLocation> Departments { get; private set; }

}