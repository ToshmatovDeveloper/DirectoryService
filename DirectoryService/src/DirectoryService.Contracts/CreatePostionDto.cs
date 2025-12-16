namespace DirectoryService.Contracts;

public record CreatePostionDto(string Name, string? Description, Guid[] DepartmentIds);