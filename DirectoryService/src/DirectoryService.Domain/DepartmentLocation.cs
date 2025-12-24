using CSharpFunctionalExtensions;
using DirectoryService.Domain.ValueObjects;
using Shared;

namespace DirectoryService.Domain;

public class DepartmentLocation
{
    public DepartmentLocation(Guid departmentId, LocationId locationId)
    {
    }

    public Guid DepartmentId { get; private set; }
    public Department Department { get; private set; } = null!;
    
    public Guid LocationId { get; private set; }
    public Location Location { get; private set; } = null!;
    
}