using MediatR;

namespace M33tingClub.Application.Utilities;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    
}