using DirectoryService.Application;
using DirectoryService.Infrastructure;
using DirectoryService.Middleware;
using DirectoryService.Presentation;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database"));

    options.UseLoggerFactory(ApplicationDbContext.MyLoggerFactory)
        .EnableSensitiveDataLogging()
        .EnableDetailedErrors();
});
builder.Services
    .AddApplication()
    .AddInfrastructureDependencies();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers()
    .AddApplicationPart(typeof(LocationsController).Assembly);

builder.Services.AddOpenApi();

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