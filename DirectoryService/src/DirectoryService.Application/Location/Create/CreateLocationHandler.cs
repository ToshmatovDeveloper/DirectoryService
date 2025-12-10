using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Validation;
using DirectoryService.Contracts;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace DirectoryService.Application.Location.Create;

public class CreateLocationHandler :ICommandHandler<Guid, CreateLocationRequest>
{
    private readonly ILocationRepository _locationRepository;
    private readonly IValidator<CreateLocationRequest> _validator;
    private readonly  ILogger<CreateLocationHandler> _logger;

    public CreateLocationHandler(
        ILocationRepository locationRepository, 
        IValidator<CreateLocationRequest> validator, 
        ILogger<CreateLocationHandler> logger)
    {
        _locationRepository = locationRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreateLocationRequest request, 
        CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await _validator.ValidateAsync(request,cancellationToken);

        if (!validationResult.IsValid)
        {
            _logger.LogWarning("Validation Failed");

            return validationResult.ToError();
            
            //return GeneralErrors.ValueIsInvalid("Location");
        }

        var locationId = Guid.NewGuid();

        var location = new Domain.Location(
            locationId,
            new Name(request.CreateLocationDto.Name),
            new Address(request.CreateLocationDto.Address.Country, 
                request.CreateLocationDto.Address.City, 
                request.CreateLocationDto.Address.Street),
            new Domain.ValueObjects.TimeZone(request.CreateLocationDto.TimeZone));

        Result<Guid, Error> result = await _locationRepository.AddAsync(location, cancellationToken);
        
        if (result.IsFailure)
        {
            return result.Error;
        }

        _logger.LogInformation("Location created with id {locationId}", locationId);

        return locationId;
    }
}
