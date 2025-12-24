namespace DirectoryService.Contracts;

public record CreateDepartmentDto(string Name, string Identifier, Guid ParentId,Guid[] LocationIds);