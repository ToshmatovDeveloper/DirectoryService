using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Create;

namespace DirectoryService.Application.Location.Create;

public record CreateLocationRequest(CreateLocationDto CreateLocationDto) : ICommand;
