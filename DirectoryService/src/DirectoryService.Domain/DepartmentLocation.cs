namespace DirectoryService.Domain;

public class DepartmentLocation
{
    public Guid DepartmentId { get; private set; }
    public Department Department { get; private set; } = null!;
    
    public Guid LocationId { get; private set; }
    public Location Location { get; private set; } = null!;
}