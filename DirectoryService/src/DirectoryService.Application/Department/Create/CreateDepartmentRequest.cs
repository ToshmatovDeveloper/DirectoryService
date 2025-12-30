using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Create;

namespace DirectoryService.Application.Department.Create;

public record CreateDepartmentRequest(CreateDepartmentDto CreateDepartmentDto) : ICommand;