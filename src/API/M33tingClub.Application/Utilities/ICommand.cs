using MediatR;

namespace M33tingClub.Application.Utilities;

public interface ICommand<out TResult> : IRequest<TResult>
{
    
}