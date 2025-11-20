namespace DirectoryService.Contracts;

public record CreateLocationDto(string name, AddressDto  address, string timeZone );

public  record AddressDto(string country, string city, string street);