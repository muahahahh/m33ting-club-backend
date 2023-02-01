using M33tingClub.Application.Utilities;
using MediatR;

namespace M33tingClub.Infrastructure.Processing;

public class UnitOfWorkBehavior<TCommand, TResponse> : IPipelineBehavior<TCommand, TResponse> where TCommand : ICommand<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TCommand command, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var result = await next();
        await _unitOfWork.CommitAsync();

        return result;
    }
}