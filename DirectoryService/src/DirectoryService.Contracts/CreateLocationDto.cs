namespace DirectoryService.Contracts;

public record CreateLocationDto(string Name, AddressDto  Address, string TimeZone );

public  record AddressDto(string Country, string City, string Street);