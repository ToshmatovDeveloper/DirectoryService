using DirectoryService.Application.Department.Create;
using DirectoryService.Application.Department.Move;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared;
using Path = DirectoryService.Domain.ValueObjects;
using TimeZone = DirectoryService.Domain.ValueObjects.TimeZone;

namespace DirectoryService.Tests;

public class MoveDepartmentsTests : DepartmentBaseTests
{
    public MoveDepartmentsTests(DepartmentTestWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Move_Department_Should_Be_Success()
    {
        //Arrange
        var locationId = await CreateLocation();

        var departmentId = await CreateDepartment(locationId);

        var cancellationToken = CancellationToken.None;

        //Act

        var result = await ExecuteHandler((sut) =>
        {
            var request = new MoveDepartmentRequest(Guid.NewGuid());
            return sut.Handle(departmentId, request, cancellationToken);
        });

        //Assert

        await ExecuteInDb(async dbContext =>
        {
            var department = await dbContext.Departments
                .FirstAsync(d => d.Id == result.Value, cancellationToken);

            Assert.NotNull(department);
            Assert.True(result.IsSuccess);
            Assert.Empty(result.Error.Messages);
        });
    }

    [Fact]
    public async Task MoveDepartment_NonExistentDepartment_ShouldFail()
    {
        //Arrange
        
        var locationId = await CreateLocation();

        var departmentId = await CreateDepartment(locationId);

        var cancellationToken = CancellationToken.None;
        
        // Act 
        var result = await ExecuteHandler(async sut =>
        {
            var request = new MoveDepartmentRequest(null);
            return await sut.Handle(departmentId, request, cancellationToken);
        });

        //Assert
        Assert.False(result.IsSuccess);

    }

    private async Task<LocationId> CreateLocation()
    {
        var locationId = LocationId.Create(Guid.NewGuid());
        //Arrange
        return await ExecuteInDb(async dbContext =>
        {
            var location = new Location(
                locationId,
                Name.Create("Location").Value,
                Address.Create("Russia", "Moscow", "Lenin").Value,
                TimeZone.Create("Central Standard Time").Value);


            dbContext.Locations.Add(location);
            await dbContext.SaveChangesAsync();

            return locationId;
        });
    }

    private async Task<Guid> CreateDepartment(LocationId locationId)
    {
        var id = Guid.NewGuid();
        var name = Name.Create("Department");
        var identifier = Identifier.Create("main");
        var path = Path.Path.Create("it");
        int depth = 0;
        Guid? parentId = null;

        var depLoc = new DepartmentLocation(id, locationId);
        var departmentLocations = new List<DepartmentLocation>
        {
            new DepartmentLocation(id, locationId),
        };

        return await ExecuteInDb(async dbContext =>
        {
            var department = new Department(
                id,
                name.Value,
                identifier.Value,
                path.Value,
                depth,
                parentId,
                departmentLocations);

            dbContext.Departments.Add(department);
            await dbContext.SaveChangesAsync();

            return id;
        });
    }

    private async Task<T> ExecuteHandler<T>(Func<MoveDepartmentHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();

        var sut = scope.ServiceProvider.GetRequiredService<MoveDepartmentHandler>();

        return await action(sut);
    }
}