using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Domain.ValueObjects;

public record Address
{
    public Address(string country, string city, string street)
    {
        Country = country;
        City = city;
        Street = street;
    }
    
    public string Country { get; }
    
    public string City { get; }
    
    public string Street { get; }

    public static Result<Address, Error> Create(string country, string city, string street)
    {
        var validation = Validate(country, city, street);
        
        if (validation.IsFailure)
            return GeneralErrors.ValueIsInvalid("Value is invalid");
        
        return new Address(country, city, street);
    }
    
    public static Result<bool,Error> Validate(string country, string city, string street)
    {
        if (string.IsNullOrWhiteSpace(country) || country.Length > 100)
            return GeneralErrors.ValueIsRequired(country);
        
        if (string.IsNullOrWhiteSpace(city) || city.Length > 100)
            return GeneralErrors.ValueIsRequired(city);  
        
        if (string.IsNullOrWhiteSpace(street) || street.Length > 100)
            return GeneralErrors.ValueIsRequired(street);        
        
        else return true;
    }
    
    public bool Equal(Address? other)
    {
        if(other is null) return false;
            
        return string.Equals(Country, other.Country, StringComparison.OrdinalIgnoreCase)
               && string.Equals(City, other.City, StringComparison.OrdinalIgnoreCase)
               && string.Equals(Street, other.Street, StringComparison.OrdinalIgnoreCase);;
    }
};