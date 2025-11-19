using DirectoryService.Application;

namespace DirectoryService;

public static class DependencyInjection 
{
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services)
    {
        services.AddWebDependencies();
        
        services.AddAplication();
        
        return services;
    }
    
    public static IServiceCollection AddWebDependencies(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
        
        return services;
    }
}