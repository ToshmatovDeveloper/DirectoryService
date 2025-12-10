using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Application.Abstractions;

public interface ICommand;

public interface ICommandHandler<TResponse, TCommand>
    where TCommand : ICommand
{
    Task<Result<Guid, Error>> Handle(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<TCommand>
    where TCommand : ICommand
{
    Task<UnitResult<Error>> Handle(TCommand command, CancellationToken cancellationToken);
}