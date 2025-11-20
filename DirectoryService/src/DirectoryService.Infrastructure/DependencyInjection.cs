using DirectoryService.Application;
using DirectoryService.Infrastructure.Repository;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
    {
        
        services.AddScoped<ILocationRepository, LocationRepository>();

        return services;
    }
}