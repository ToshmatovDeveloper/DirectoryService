using CSharpFunctionalExtensions;
using DirectoryService.Application.Abstractions;
using DirectoryService.Application.Validation;
using DirectoryService.Contracts;
using DirectoryService.Domain;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Shared;

namespace DirectoryService.Application.Position.Create;

public class CreatePositionHandler : ICommandHandler<Guid, CreatePositionRequest>
{
    private readonly IPositionRepository _positionRepository;
    private readonly IValidator<CreatePositionRequest> _validator;
    private readonly ILogger<CreatePositionHandler> _logger;

    public CreatePositionHandler(IPositionRepository positionRepository, IValidator<CreatePositionRequest> validator, ILogger<CreatePositionHandler> logger)
    {
        _positionRepository = positionRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, Error>> Handle(
        CreatePositionRequest request, 
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request,  cancellationToken);
        if (!validationResult.IsValid)
        {
            _logger.LogError($"Validation Error: {validationResult.Errors}");
            return validationResult.ToError();
        }
        
        var nameResult = Name.Create(request.CreatePostionDto.Name);
        if (nameResult.IsFailure)
        {
            _logger.LogError("Invalid Name");
            return nameResult.Error;
        }
        
        var descriptionResult = request.CreatePostionDto.Description;
        
        var positionId = Guid.NewGuid();

        var departmentPositions = request.CreatePostionDto.DepartmentIds
            .Select(positionId => new DepartmentPosition(
                new DepartmentId(Guid.NewGuid()),
                new PositionId(positionId)))
            .ToList();
        
        Result<Domain.Position,  Error> positionResult;

        positionResult = Domain.Position.Create(
            positionId,
            nameResult.Value,
            descriptionResult,
            departmentPositions);

        if (positionResult.IsFailure)
        {
            _logger.LogError("Ошибка при создание новой должности!");
            return positionResult.Error;
        }
        
        await _positionRepository.AddAsync(positionResult.Value, cancellationToken);

        _logger.LogInformation("Department created successfully with id {departmentId}", positionId);

        return positionId;

    }
}