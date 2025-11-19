using DirectoryService.Application.Location;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Application;

public static class DependencyInjection 
{
    public static IServiceCollection AddAplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        services.AddScoped<ILocationsService, LocationsService>();
        
        return services;
    }
}