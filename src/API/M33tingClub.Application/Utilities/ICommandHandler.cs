using MediatR;

namespace M33tingClub.Application.Utilities;

public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    
}