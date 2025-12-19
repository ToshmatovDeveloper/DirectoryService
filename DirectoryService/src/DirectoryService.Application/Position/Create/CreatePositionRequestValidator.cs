using DirectoryService.Application.Validation;
using DirectoryService.Domain.ValueObjects;
using FluentValidation;
using Shared;

namespace DirectoryService.Application.Position.Create;

public class CreatePositionRequestValidator :AbstractValidator<CreatePositionRequest>
{
    public CreatePositionRequestValidator()
    {
        RuleFor(x => x.CreatePostionDto.Name)
            .MustBeValueObject(Name.Create);

        RuleFor(x => x.CreatePostionDto.Description)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired("position.Description"))
            .MaximumLength(1000)
            .WithError(GeneralErrors.ValueIsInvalid("position.Description"));

        RuleFor(x => x.CreatePostionDto.DepartmentIds)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired("position.DepartmentIds"))
            .Must(departmentIds => departmentIds != null
                                   && departmentIds.Count() == departmentIds.Distinct().Count());
    }
}