using DirectoryService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<ApplicationDbContext>(_ => 
    new ApplicationDbContext(builder.Configuration.GetConnectionString("DirectoryServiceDb")!));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();
