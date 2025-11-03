namespace DirectoryService.Domain;

public class Position
{
    private Position(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length < 3 || name.Length > 100)
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
    
    public string Name { get; private set; }
    
    public string Description { get; private set; }
    
    public bool IsActive { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
}