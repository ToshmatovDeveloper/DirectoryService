using DirectoryService.Domain.ValueObjects;
using TimeZone = DirectoryService.Domain.ValueObjects.TimeZone;

namespace DirectoryService.Contracts.Location;

public record CreateLocationDto(Name Name, Address Address, TimeZone TimeZone);