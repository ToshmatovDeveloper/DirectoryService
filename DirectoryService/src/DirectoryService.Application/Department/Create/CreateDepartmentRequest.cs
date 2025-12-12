using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts;

namespace DirectoryService.Application.Department.Create;

public record CreateDepartmentRequest(CreateDepartmentDto CreateDepartmentDto) : ICommand;