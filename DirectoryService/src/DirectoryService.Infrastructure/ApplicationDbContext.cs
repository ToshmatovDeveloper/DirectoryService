using DirectoryService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public static readonly ILoggerFactory MyLoggerFactory =
        LoggerFactory.Create(builder =>
        {
            builder
                .AddConsole()
                .AddFilter((category, level) =>
                    category == DbLoggerCategory.Database.Command.Name &&
                    level == LogLevel.Information);
        });
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
    
    public DbSet<Department> Departments { get; set; }
    public DbSet<DepartmentLocation> DepartmentLocations { get; set; }
    public DbSet<DepartmentPosition> DepartmentPositions { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Position> Positions { get; set; }
}