namespace Shared;

public static class GeneralErrors
{
    public static Error ValueIsInvalid(string? name = null)
    {
        string label = name ?? "value";
        return Error.Validation(new ErrorMessage("value.is.invalid",
            $"{label} is invalid",name));
    }
    
    public static Error NotFound(Guid? id = null, string? name = null)
    {
        string forId = id == null ? string.Empty : $"no Id {id}";
        return Error.NotFound(new ErrorMessage("record.not.found", 
            $"{name ?? "record"} not found {forId}",name));
    }
    
    public static Error ValueIsRequired(string? name = null)
    {
        string label = name == null ? string.Empty : "" + name + "";
        return Error.Validation(new ErrorMessage("length.is.invalid", 
            $"Field {label} is required",name));
    }

    public static Error AlreadyExist()
    {
        return Error.Conflict(new ErrorMessage("record.already_exist","already exist", null));
    }

    public static Error Failure(string? message = null)
    {
        return Error.Failure(new ErrorMessage("server.failure", message ?? "server.failure",null));
    }
}