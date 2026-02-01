namespace DirectoryService.Contracts.Create;

public record CreateDepartmentDto(string Name, string Identifier, Guid? ParentId,Guid[] LocationIds);