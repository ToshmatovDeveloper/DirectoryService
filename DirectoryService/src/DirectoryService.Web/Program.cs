using DirectoryService;
using DirectoryService.Infrastructure;
using DirectoryService.Middleware;
using DirectoryService.Presentation;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddScoped<ApplicationDbContext>(_ =>
    new ApplicationDbContext(builder.Configuration.GetConnectionString("DirectoryServiceDb")!));

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