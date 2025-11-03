namespace DirectoryService.Domain;

public class DepartmentLocation
{
    public Guid DepartmentId { get; set; }
    public Department Department { get; set; } = null!;
    
    public Guid LocationId { get; set; }
    public Location Location { get; set; } = null!;
}