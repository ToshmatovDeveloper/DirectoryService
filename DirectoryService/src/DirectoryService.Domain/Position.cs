using CSharpFunctionalExtensions;
using DirectoryService.Domain.ValueObjects;
using Shared;

namespace DirectoryService.Domain;

public record Position
{
    private Position()
    {
        
    }

    private Position(
        Guid id,
        Name name,
        string description,
        IEnumerable<DepartmentPosition> departments)
    {
        Id = id;
        Name = name;
        Description = description;
        Departments = departments;
    }

    public Guid Id { get; private set; }
    
    public Name Name { get; private set; }
    
    public string Description { get; private set; }
    
    public bool IsActive { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
    
    public DateTime DeletedAt { get; private set; }
    
    public IEnumerable<DepartmentPosition> Departments { get; private set; }

    public static Result<Position, Error> Create(
        Guid id,
        Name name, 
        string description,
        IEnumerable<DepartmentPosition> departments)
    {
        var positionDepartmentsList = departments.ToList();
        
        return new Position(
             id,
             name,
             description,
             positionDepartmentsList);
    }
    
    public bool SoftDelete()
    {
        DeletedAt = DateTime.UtcNow;
        return IsActive = false;
    }
    
    public bool PositionHasOneDepartment()
    {
        if (Departments.Count() == 1)
            return true;
        
        return false;
    }
}