namespace DirectoryService.Domain;

public record Position
{
    private Position()
    {
        
    }
    private Position(Name name, string description)
    {
        if (string.IsNullOrWhiteSpace(name.ToString()) || name.ToString().Length < 3 || name.ToString().Length > 100)
        {
            throw new Exception("Wrong position name");
        }
        
        Name = name;

        if (description.Length >= 1000)
        {
            throw new Exception("Position description mast be less than 1000 characters");
        }
        
        Description = description;
    }
    
    public Guid Id { get; private set; }
    
    public Name Name { get; private set; }
    
    public string Description { get; private set; }
    
    public bool IsActive { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
    
    public List<DepartmentPosition> Departments { get; private set; }
}