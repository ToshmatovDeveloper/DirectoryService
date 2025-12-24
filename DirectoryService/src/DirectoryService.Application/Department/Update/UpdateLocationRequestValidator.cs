using DirectoryService.Application.Validation;
using FluentValidation;
using Shared;

namespace DirectoryService.Application.Department.Update;

public class UpdateLocationRequestValidator : AbstractValidator<UpdateLocationRequest>
{
    public UpdateLocationRequestValidator()
    {
        RuleFor(x => x.UpdateLocationDto.LocationIds)
            .NotEmpty()
            .WithError(GeneralErrors.ValueIsRequired("locationIds"))
            .Must(locationIds => locationIds != null
                                 && locationIds.Count() == locationIds.Distinct().Count())
            .WithError(GeneralErrors.ValueIsRequired("locationIds"));
    }
}