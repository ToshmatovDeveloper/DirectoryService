using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts;
using DirectoryService.Contracts.Create;

namespace DirectoryService.Application.Position.Create;

public record CreatePositionRequest(CreatePostionDto CreatePostionDto) : ICommand;