using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts;

namespace DirectoryService.Application.Location.Create;

public record CreateLocationRequest(CreateLocationDto CreateLocationDto) : ICommand;
