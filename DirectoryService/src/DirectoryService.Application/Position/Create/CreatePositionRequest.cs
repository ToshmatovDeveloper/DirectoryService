using DirectoryService.Application.Abstractions;
using DirectoryService.Contracts;

namespace DirectoryService.Application.Position.Create;

public record CreatePositionRequest(CreatePostionDto CreatePostionDto) : ICommand;