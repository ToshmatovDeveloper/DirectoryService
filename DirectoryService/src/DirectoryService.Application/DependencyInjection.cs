using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Department.Create;
using DirectoryService.Application.Location;
using DirectoryService.Application.Location.Create;
using DirectoryService.Application.Position.Create;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared;

namespace DirectoryService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        services.AddScoped<ICommandHandler<Guid, CreateLocationRequest>, CreateLocationHandler>();
        services.AddScoped<ICommandHandler<Guid, CreateDepartmentRequest>, CreateDepartmentHandler>();
        services.AddScoped<ICommandHandler<Guid, CreatePositionRequest>, CreatePositionHandler>();

        return services;
    }
}