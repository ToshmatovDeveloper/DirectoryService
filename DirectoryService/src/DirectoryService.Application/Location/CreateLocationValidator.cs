using DirectoryService.Contracts;
using FluentValidation;

namespace DirectoryService.Application;

public class CreateLocationValidator : AbstractValidator<CreateLocationDto>
{
    public CreateLocationValidator()
    {
        RuleFor(x => x.name)
            .NotEmpty()
            .WithMessage("Name is required");

        RuleFor(x => x.address.country)
            .NotEmpty()
            .WithMessage("Country is required")
            .MaximumLength(100)
            .WithMessage("Country must not exceed 100 characters");

        RuleFor(x => x.address.city)
            .NotEmpty()
            .WithMessage("City is required")
            .MaximumLength(100)
            .WithMessage("City must not exceed 100 characters");

        RuleFor(x => x.address.street)
            .NotEmpty()
            .WithMessage("Street is required")
            .MaximumLength(100)
            .WithMessage("Street must not exceed 100 characters");

        RuleFor(x => x.timeZone)
            .NotEmpty()
            .WithMessage("Часовой пояс не может быть пустым!")
            .Must(IsValidTimeZone)
            .WithMessage("Указанный часовой пояс не существует");
    }

    private bool IsValidTimeZone(TimeZone arg)
    {
        try
        {
            TimeZoneInfo.FindSystemTimeZoneById(arg.ToString());
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