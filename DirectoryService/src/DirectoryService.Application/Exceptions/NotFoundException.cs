namespace DirectoryService.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(Guid id) 
        : base($"Record with id: {id} was not found.")
    {
        
    }
}