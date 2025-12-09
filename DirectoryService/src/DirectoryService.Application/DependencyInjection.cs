using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Location;
using DirectoryService.Application.Location.Create;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace DirectoryService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        //services.AddScoped<ILocationsService, LocationsService>();
        
        services.AddScoped<ICommandHandler<Guid, CreateLocationRequest>, CreateLocationHandler>();

        return services;
    }
}