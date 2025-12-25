using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts.Update;

namespace DirectoryService.Application.Department.Update;

public record UpdateLocationRequest(Guid DepartmentId, IEnumerable<Guid> LocationsId) : ICommand;