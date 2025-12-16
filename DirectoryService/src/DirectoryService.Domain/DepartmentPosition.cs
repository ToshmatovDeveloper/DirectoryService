using DirectoryService.Domain.ValueObjects;

namespace DirectoryService.Domain;

public class DepartmentPosition
{
    public DepartmentPosition(DepartmentId departmentId, PositionId positionId)
    {
    }
    
    public Guid DepartmentId { get; private set; }
    public Department Department { get; private set; } = null!;
    
    public Guid PositionId { get; private set; }
    public Position Position { get; private set; } = null!;

}