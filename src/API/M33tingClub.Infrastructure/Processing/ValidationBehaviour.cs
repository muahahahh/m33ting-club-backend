using FluentValidation;
using MediatR;

namespace M33tingClub.Infrastructure.Processing;

public class ValidationBehaviour<TRequest, TResponse> 
	: IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
	{
		_validators = validators;
	}
	
	public async Task<TResponse> Handle(
		TRequest request,
		CancellationToken cancellationToken,
		RequestHandlerDelegate<TResponse> next)
	{
		var failures = _validators
           .Select(v => v.Validate(request))
           .SelectMany(result => result.Errors)
           .Where(f => f != null)
           .ToList();

		if (failures.Any())
		{
			throw new ValidationException(failures);
		}

		return await next();
	}
}