using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.Domain;

public class DepartmentLocation
{
    public DepartmentLocation(DepartmentId departmentId, LocationId locationId)
    {
    }

    public Guid DepartmentId { get; private set; }
    public Department Department { get; private set; } = null!;
    
    public Guid LocationId { get; private set; }
    public Location Location { get; private set; } = null!;
}