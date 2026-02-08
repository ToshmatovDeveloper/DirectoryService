namespace DirectoryService.Application.Abstractions;

public interface IReadDbContext
{
    IQueryable<Domain.Location>  LocationsRead { get; }
    IQueryable<Domain.Department>  DepartmentsRead { get; }
    IQueryable<Domain.Position>  PositionsRead { get; }
}