using System.Net;
using FluentValidation;
using M33tingClub.Domain.Utilities;
using M33tingClub.Domain.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;

namespace M33tingClub.Web.ExceptionHandling;

internal static class ExceptionMapper
{
	internal static ErrorResponse ToErrorResponse(this Exception e)
	{
		return e switch
		{
			ValidationException validationException => new ErrorResponse(
				validationException.Errors.Select(x => x.ErrorMessage),
				HttpStatusCode.BadRequest),
			
			RuleValidationException ruleException 
				when ruleException.BrokenRule.Kind == RuleExceptionKind.Conflict => new ErrorResponse(
				ruleException.Message,
				HttpStatusCode.Conflict),
			
			RuleValidationException ruleException 
				when ruleException.BrokenRule.Kind == RuleExceptionKind.BadRequest => new ErrorResponse(
					ruleException.Message,
					HttpStatusCode.BadRequest),

			ObjectNotFoundException objectNotFoundException => new ErrorResponse(
				objectNotFoundException.Message,
				HttpStatusCode.NotFound),

			DbUpdateConcurrencyException _ => new ErrorResponse(
				"Operation was not successful due to concurrency conflict.",
				HttpStatusCode.Conflict),
			
			UserNotExistsException userNotInDatabaseException => new ErrorResponse(
				userNotInDatabaseException.Message,
				HttpStatusCode.Unauthorized),

			_ => new ErrorResponse(
				"Internal server error.",
				HttpStatusCode.InternalServerError)
		};
	}
}