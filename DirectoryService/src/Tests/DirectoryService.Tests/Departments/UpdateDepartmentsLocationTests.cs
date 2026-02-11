using DirectoryService.Application.Department.Update;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects;
using DirectoryService.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Path = DirectoryService.Domain.ValueObjects.Path;
using TimeZone = DirectoryService.Domain.ValueObjects.TimeZone;

namespace DirectoryService.Tests.Departments;

public class UpdateDepartmentsLocationTests : DepartmentBaseTests
{
    public UpdateDepartmentsLocationTests(DepartmentTestWebFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Update_departments_location_should_success()
    {
        //Arrange

        var locationId = await CreateLocation();

        IEnumerable<Guid> locationsId = new[] { locationId };

        
        var departmentId = await CreateDepartment(locationId);

        var cancellationToken = CancellationToken.None;

        //Act
        
        var result = await ExecuteHandler((sut) =>
        {
            var request = new UpdateLocationRequest(departmentId, locationsId);
            return sut.Handle(request, cancellationToken);
        });
        
        Console.WriteLine($"Result IsSuccess: {result.IsSuccess}");
    
        if (!result.IsSuccess && result.Error != null)
        {
            Console.WriteLine($"Error Type: {result.Error.Type}");
            Console.WriteLine($"Error Messages: {string.Join(", ", result.Error.Messages)}");
        }

        //Assert
        
        Assert.True(result.IsSuccess, 
            result.IsFailure 
                ? $"Failed with error: {result.Error}" 
                : "Expected success");    
    }
    
    
    
    private async Task<Guid> CreateLocation()
    {
        var locationId = Guid.NewGuid();
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

    private async Task<Guid> CreateDepartment(Guid locationId)
    {
        var id = Guid.NewGuid();
        var name = Name.Create("Department");
        var identifier = Identifier.Create("main");
        var path = Path.Create("it");
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
    
    private async Task<T> ExecuteHandler<T>(Func<UpdateLocationHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();

        var sut = scope.ServiceProvider.GetRequiredService<UpdateLocationHandler>();

        return await action(sut);
    }

}

