namespace DirectoryService.Domain;

public record Address
{
    public Address(string country, string city, string street)
    {
        if(!IsValid(country,city,street))
            throw new ArgumentException("Invalid country or city or street");
        
        Country = country;
        City = city;
        Street = street;
    }
    
    public string Country { get; }
    
    public string City { get; }
    
    public string Street { get; }

    public bool IsValid(string country, string city, string street)
    {
        if (string.IsNullOrWhiteSpace(country))
            throw new ArgumentNullException("Country is required");
        
        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentNullException("City is required");
        
        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentNullException("Street is required");
        
        if(country.Length > 100) throw new ArgumentOutOfRangeException("Country is too long");
        
        if (city.Length > 100) throw new ArgumentOutOfRangeException("City is too long");
        
        if (street.Length > 100) throw new ArgumentOutOfRangeException("Street is too long");

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