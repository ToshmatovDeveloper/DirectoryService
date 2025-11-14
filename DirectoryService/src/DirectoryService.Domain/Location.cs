namespace DirectoryService.Domain;

public record Location
{
    private Location()
    {
        
    }

    private Location(Name name)
    {
        if (string.IsNullOrWhiteSpace(name.ToString()) || name.ToString().Length < 3 || name.ToString().Length > 120)
        {
            throw new Exception("Wrong position name");
        }
        
        Name = name;
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