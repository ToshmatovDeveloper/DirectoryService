using DirectoryService.Application;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Department;
using DirectoryService.Application.Location;
using DirectoryService.Application.Locations;
using DirectoryService.Application.Position;
using DirectoryService.Infrastructure.Database;
using DirectoryService.Infrastructure.Factory;
using DirectoryService.Infrastructure.Repository;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
    {
        services.AddScoped<ApplicationDbContext>();
        services.AddScoped<IReadDbContext>();
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IDepartmentRepository, DepartmentRepository>();
        services.AddScoped<IPositionRepository, PositionRepository>();
        services.AddScoped<ITransactionManager, TransactionManager>();

        return services;
    }
}