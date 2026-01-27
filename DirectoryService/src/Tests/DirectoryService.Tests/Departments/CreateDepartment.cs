using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Department.Create;
using DirectoryService.Contracts.Create;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects;
using DirectoryService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TimeZone = DirectoryService.Domain.ValueObjects.TimeZone;

namespace DirectoryService.Tests;

public class CreateDepartment : DirectoryBaseTests
{
    public CreateDepartment(DepartmentTestWebFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task CreateDepartment_with_valid_data_should_succeed()
    {
        //Arrange
        var locationId = await CreateLocation();
        
        var cancellationToken = CancellationToken.None;

        //Act
        var result = await ExecuteHandler((sut) =>
        {
            var command = new CreateDepartmentRequest(
                new CreateDepartmentDto("Department", "dep", null, [locationId]));

            return sut.Handle(command, cancellationToken);
        });

        //Assert
        await ExecuteInDb(async dbContext =>
        {
            var department = await dbContext.Departments
                .FirstAsync(d => d.Id == result.Value, cancellationToken);

            Assert.NotNull(department);
            Assert.Equal(result.Value, department.Id);

            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value);
        });
    }
    
    [Fact]
    public async Task CreateTwoDepartment_with_valid_data_should_succeed()
    {
        //Arrange
        var firstLocationId = await CreateLocation();
        var secondLocationId = await CreateLocation();
        
        var cancellationToken = CancellationToken.None;

        //Act
        var result = await ExecuteHandler((sut) =>
        {
            var command = new CreateDepartmentRequest(
                new CreateDepartmentDto("Department", "dep", null, [firstLocationId, secondLocationId]));

            return sut.Handle(command, cancellationToken);
        });

        //Assert
        await ExecuteInDb(async dbContext =>
        {
            var department = await dbContext.Departments
                .FirstAsync(d => d.Id == result.Value, cancellationToken);

            Assert.NotNull(department);
            Assert.Equal(result.Value, department.Id);

            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value);
        });
    }

    [Fact]
    public async Task CreateDepartment_with_non_existent_locationId_should_failed()
    {
        //Arrange
        var locationId = Guid.Empty;
        
        var cancellationToken = CancellationToken.None;

        //Act
        var result = await ExecuteHandler((sut) =>
        {
            var command = new CreateDepartmentRequest(
                new CreateDepartmentDto("Department", "dep", null, [locationId]));

            return sut.Handle(command, cancellationToken);
        });

        //Assert
        await ExecuteInDb(async dbContext =>
        {
            var department = await dbContext.Departments
                .FirstAsync(d => d.Id == result.Value, cancellationToken);

            Assert.Null(department);
            Assert.NotEqual(result.Value, department?.Id);

            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Error.Messages);
        });
    }
    
    [Fact]
    public async Task CreateDepartment_with_invalid_data_should_failed()
    {
        //Arrange
        var locationId = await CreateLocation();

        var cancellationToken = CancellationToken.None;

        //Act
        var result = await ExecuteHandler((sut) =>
        {
            var command = new CreateDepartmentRequest(
                new CreateDepartmentDto("", "dep", null, [locationId]));

            return sut.Handle(command, cancellationToken);
        });

        //Assert
        await ExecuteInDb(async dbContext =>
        {
            var department = await dbContext.Departments
                .FirstAsync(d => d.Id == result.Value, cancellationToken);

            Assert.Null(department);
            Assert.NotEqual(result.Value, department?.Id);

            Assert.True(result.IsFailure);
            Assert.NotEmpty(result.Error.Messages);
        });
    }

    private async Task<Guid> CreateLocation()
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

            return locationId.Value;
        });
    }

    private async Task<T> ExecuteHandler<T>(Func<CreateDepartmentHandler, Task<T>> action)
    {
        await using var scope = Services.CreateAsyncScope();

        var sut = scope.ServiceProvider.GetRequiredService<CreateDepartmentHandler>();

        return await action(sut);
    }

    
}