using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Department.Create;
using DirectoryService.Application.Department.Move;
using DirectoryService.Application.Department.Query;
using DirectoryService.Application.Department.Update;
using DirectoryService.Application.Location;
using DirectoryService.Application.Location.Create;
using DirectoryService.Application.Location.Query;
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
        services.AddScoped<UpdateLocationHandler>();
        services.AddScoped<GetLocationByIdHandler>();
        services.AddScoped<GetLocationsHandler>();
        services.AddScoped<GetDepartmentsHandler>();
        services.AddScoped<MoveDepartmentHandler>();

        return services;
    }
}