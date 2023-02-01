using MediatR;

namespace M33tingClub.Application.Utilities;

public interface IQuery<out TResult> : IRequest<TResult>
{
    
}