namespace DirectoryService.Contracts.Create;

public record CreatePostionDto(string Name, string? Description, Guid[] DepartmentIds);