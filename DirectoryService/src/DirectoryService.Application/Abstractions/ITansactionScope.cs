using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Application.Abstractions;

public interface ITansactionScope : IDisposable
{
    UnitResult<Error> Commit(); 
    UnitResult<Error> Rollback(); 
}