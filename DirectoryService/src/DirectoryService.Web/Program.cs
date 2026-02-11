using DirectoryService;
using DirectoryService.Application;
using DirectoryService.Application.Abstractions;
using DirectoryService.Infrastructure;
using DirectoryService.Middleware;
using DirectoryService.Presentation;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services
    .AddProgramDependencies()
    .AddEndpointsApiExplorer()
    .AddOpenApi()
    .AddControllers()
    .AddApplicationPart(typeof(LocationsController).Assembly);

var app = builder.Build();

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options => { options.SwaggerEndpoint("/openapi/v1.json", "DirectoryService"); });
}

app.MapControllers();

app.Run();

namespace DirectoryService
{
    public partial class Program;
}