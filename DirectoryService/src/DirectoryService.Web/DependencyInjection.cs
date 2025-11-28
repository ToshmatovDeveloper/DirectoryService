using DirectoryService.Application;
using DirectoryService.Infrastructure;
using Microsoft.OpenApi.Models;
using Shared;

namespace DirectoryService;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services)
    {
        services.AddApplication();
        services.AddInfrastructureDependencies();
        services.AddWebDependencies();
        
        return services;
    }

    public static IServiceCollection AddWebDependencies(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
        return services;
    }
}