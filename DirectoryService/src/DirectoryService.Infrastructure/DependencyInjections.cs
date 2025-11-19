using DirectoryService.Application.Location;
using DirectoryService.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Infrastructure;

public static class DependencyInjection 
{
    public static IServiceCollection AddAplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        services.AddScoped<ILocationRepository, LocationRepository>();
        
        return services;
    }
}