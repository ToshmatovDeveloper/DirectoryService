namespace DirectoryService.Contracts.Get;

public record GetLocationsDto(List<LocationDto> Locations, long TotalCount);