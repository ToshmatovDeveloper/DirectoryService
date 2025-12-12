using DirectoryService.Application.Validation;
using FluentValidation;
using DirectoryService.Domain.ValueObjects;
using TimeZone = DirectoryService.Domain.ValueObjects.TimeZone;

namespace DirectoryService.Application.Location.Create;

public class CreateLocationRequestValidator : AbstractValidator<CreateLocationRequest>
{
    public CreateLocationRequestValidator()
    {
        RuleFor(x => x.CreateLocationDto.Name)
            .MustBeValueObject(Name.Create);
        
        RuleFor(x => x.CreateLocationDto.TimeZone)
            .MustBeValueObject(TimeZone.Create);
        
        RuleFor(x => x.CreateLocationDto.Address)
            .Custom((address, context) =>
            {
                var validationResult = Address.Validate(
                    address.Country,
                    address.City,
                    address.Street
                );
                
                if (validationResult.IsFailure)
                {
                    context.AddFailure("Address", 
                        validationResult.Error.Messages.ToString());
                }
            });
    }
}