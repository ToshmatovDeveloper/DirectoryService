using DirectoryService.Application;
using DirectoryService.Application.Department;
using DirectoryService.Application.Location;
using DirectoryService.Application.Position;
using DirectoryService.Infrastructure.Repository;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DirectoryService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
    {
        
        services.AddDbContext<ApplicationDbContext>();

        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();

        return services;
    }
}