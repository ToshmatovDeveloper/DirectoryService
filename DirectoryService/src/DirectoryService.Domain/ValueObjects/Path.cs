using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared;

namespace DirectoryService.Domain.ValueObjects;

public record Path
{
    public Path(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<Path, Error> PathValidator(Path? parentPath, Identifier identifier)
    {
        if (parentPath == null)
        {
            var createPath = Create(identifier.Value);
            if (!createPath.IsSuccess)
                return createPath.Error;
            return createPath.Value;
        }
        
        string newPath = parentPath.Value + "." + identifier.Value;
        var newPathCreateResult = Create(newPath);
        if (!newPathCreateResult.IsSuccess)
            return newPathCreateResult.Error;
        
        return newPathCreateResult.Value;
    }
    
    public static Result<Path, Error> CreateParent(Identifier identifier)
    {
        Result<Path, Error> newPath = Path.Create(identifier.Value);
        if (newPath.IsFailure)
            return newPath.Error;

        return newPath.Value;
    }

    
    public Path CreateChild(Identifier childIdentifier)
    {
        return new Path(Value + "." + childIdentifier.Value);
    }
    
    public static Result<Path, Error> Create(string value)
    {
        var validate =  Validate(value);
        
        if (validate.IsFailure)
            return GeneralErrors.ValueIsInvalid("Value is invalid");

        return new Path(value);
    }
    
    public static Result<bool,Error> Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return GeneralErrors.ValueIsRequired(null);
      
        return true;
    }

    public static Result<Path, Error> CalculatePath(Path parentPath, Identifier chldIdentifier)
    {
        var newPath = Path.Create(parentPath.Value + "." + chldIdentifier.Value);
        if (newPath.IsFailure)
            return Error.Failure();
        
        return newPath;
    }
    
}