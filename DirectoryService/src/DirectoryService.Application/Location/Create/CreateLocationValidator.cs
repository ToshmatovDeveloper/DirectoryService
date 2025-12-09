using DirectoryService.Contracts;
using FluentValidation;

namespace DirectoryService.Application.Location.Create;

public class CreateLocationValidator : AbstractValidator<CreateLocationDto>
{
    public CreateLocationValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required");

        RuleFor(x => x.Address.Country)
            .NotEmpty()
            .WithMessage("Country is required")
            .MaximumLength(100)
            .WithMessage("Country must not exceed 100 characters");

        RuleFor(x => x.Address.City)
            .NotEmpty()
            .WithMessage("City is required")
            .MaximumLength(100)
            .WithMessage("City must not exceed 100 characters");

        RuleFor(x => x.Address.Street)
            .NotEmpty()
            .WithMessage("Street is required")
            .MaximumLength(100)
            .WithMessage("Street must not exceed 100 characters");

        RuleFor(x => x.TimeZone)
            .NotEmpty()
            .WithMessage("Часовой пояс не может быть пустым!")
            .Must(IsValidTimeZone)
            .WithMessage("Указанный часовой пояс не существует");
    }
    

    private bool IsValidTimeZone(string timeZone)
    {
        try
        {
            TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            return true;
        }
        catch (TimeZoneNotFoundException)
        {
            return false;
        }
        catch (InvalidTimeZoneException)
        {
            return false;
        }
    }
}    