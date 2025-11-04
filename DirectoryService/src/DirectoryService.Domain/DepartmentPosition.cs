namespace DirectoryService.Domain;

public class DepartmentPosition
{
    public Guid DepartmentId { get; private set; }
    public Department Department { get; private set; } = null!;
    
    public Guid PositionId { get; private set; }
    public Position Position { get; private set; } = null!;

}