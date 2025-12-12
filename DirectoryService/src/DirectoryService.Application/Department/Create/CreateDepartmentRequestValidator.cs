using DirectoryService.Application.Validation;
using DirectoryService.Contracts;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using Shared;

namespace DirectoryService.Application.Department.Create;

public class CreateDepartmentRequestValidator : AbstractValidator<CreateDepartmentRequest>
{
    public CreateDepartmentRequestValidator()
    {
        RuleFor(x => x.CreateDepartmentDto.Name)
            .MustBeValueObject(Name.Create);

        RuleFor(x => x.CreateDepartmentDto.Identifier)
            .MustBeValueObject(Identifier.Create);

        RuleFor(command => command.CreateDepartmentDto.LocationIds)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired("locationIds"))
            .Must(locationIds => locationIds != null 
                                 && locationIds.Count() == locationIds.Distinct().Count())
            .WithError(GeneralErrors.ValueIsRequired("locationIds"));

    }
    
}