using CSharpFunctionalExtensions;
using DirectoryService.Domain.ValueObjects;
using Shared;
using Path = DirectoryService.Domain.ValueObjects.Path;

namespace DirectoryService.Domain;

public sealed class Department
{
    public Guid Id { get; private set; } 

    public Name Name { get; private set; } 

    public Identifier Identifier { get; private set; } 

    public Path Path { get; private set; }

    public Guid? ParentId { get; private set; }

    public int Depth { get; private set; }

    public int ChildrenCount { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }
    
    public Department? Parent { get; private set; }
    
    public List<Department> Children { get; private set; }
    
    public List<Guid>? PositionsId { get; private set; }
    
    public List<Location> LocationId { get; private set; }
    
    /// <summary>
    /// Added list of departmentPosition and departmentLocation
    /// </summary>
    public List<DepartmentPosition> Positions { get; private set; }
    
    public List<DepartmentLocation> Locations { get; private set; }
    
    public Department(
        Guid id,
        Name name,
        Identifier identifier,
        Path path,
        int depth,
        Guid? parentId,
        IEnumerable<DepartmentLocation> departmentLocations)
    {
        Id = id;
        Name = name;
        Path = path;
        Identifier = identifier;
        ParentId = parentId;
        Depth = depth;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        DeletedAt = null;
    }

    public Department(
        Guid id,
        Name name,
        Identifier identifier,
        Path path,
        int depth,
        Guid? parentId,
        bool isActive,
        IEnumerable<DepartmentLocation> departmentLocations)
    {
        Id = id;
        Name = name;
        Path = path;
        Identifier = identifier;
        ParentId = parentId;
        Depth = depth;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        DeletedAt = DateTime.UtcNow - TimeSpan.FromDays(50);
    }

    public static Result<Department, Error> CreateParent(
        Name name,
        Identifier identifier,
        IEnumerable<DepartmentLocation> departmentLocations,
        Guid? departmentId = null!)
    {
       var departmentLocationsList = departmentLocations.ToList();

       if(departmentLocationsList.Count == 0)
           return Error.Validation(new ErrorMessage(
               "department.location",
               "Department locations must contain at least one location",
               "deparmentLocationList"));

       var path = Path.CreateParent(identifier);

       return new Department(
           departmentId ?? Guid.NewGuid(),
           name,
           identifier,
           path,
           0,
           null,
           departmentLocationsList);
    }

    public static Result<Department, Error> CreateChild(
        Name name,
        Identifier identifier,
        Department parent,
        IEnumerable<DepartmentLocation> departmentLocations,
        Guid? departmentId = null!)
    {
        var departmentLocationsList = departmentLocations.ToList();

        if(departmentLocationsList.Count == 0)
            return Error.Validation(new ErrorMessage(
                "department.location",
                "Department locations must contain at least one location",
                "deparmentLocationsList"));

        var path = parent.Path.CreateChild(identifier);

        return new Department(
            departmentId ??  Guid.NewGuid(),
            name,
            identifier,
            path,
            parent.Depth + 1,
            parent.Id,
            departmentLocationsList);
    }

    public static Result<Department, Error> CreateInactiveParent(
        Name name,
        Identifier identifier,
        IEnumerable<DepartmentLocation> departmentLocations,
        Guid? departmentId = null!)
    {
        var departmentLocationsList = departmentLocations.ToList();

        if(departmentLocationsList.Count == 0)
            return Error.Validation(new ErrorMessage(
                "department.location",
                "Department locations must contain at least one location",
                "deparmentLocationList"));

        var path = Path.CreateParent(identifier);

        return new Department(
            departmentId ?? Guid.NewGuid(),
            name,
            identifier,
            path,
            0,
            null,
            false,
            departmentLocationsList);
    }

    public static Result<Department, Error> CreateInactiveChild(
        Name name,
        Identifier identifier,
        Department parent,
        IEnumerable<DepartmentLocation> departmentLocations,
        Guid? departmentId = null!)
    {
        var departmentLocationsList = departmentLocations.ToList();

        if(departmentLocationsList.Count == 0)
            return Error.Validation(new ErrorMessage(
                "department.location",
                "Department locations must contain at least one location",
                "deparmentLocationList"));

        var path = parent.Path.CreateChild(identifier);

        return new Department(
            departmentId ?? Guid.NewGuid(),
            name,
            identifier,
            path,
            parent.Depth + 1,
            parent.Id,
            false,
            departmentLocationsList);
    }

    public void SetLocations(IEnumerable<DepartmentLocation> departmentLocations)
    {
        Locations.Clear();
        Locations.AddRange(departmentLocations);
    }
}