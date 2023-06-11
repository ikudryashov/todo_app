using System.Net;
using FluentValidation;
using MediatR;
using TodoApp.Application.Common.Exceptions;

namespace TodoApp.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
	where TRequest: IRequest<TResponse>
{
	private readonly IValidator<TRequest>? _validator;

	public ValidationBehavior(IValidator<TRequest>? validator)
	{
		_validator = validator;
	}

	public async Task<TResponse> Handle(TRequest request,
		RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		if (_validator is null)
		{
			return await next();
		}
		
		var validationResult = await _validator.ValidateAsync(request, cancellationToken);

		if (validationResult.IsValid)
		{
			return await next();
		}

		throw new RequestValidationException("Invalid request.",
			"One or more request validation errors occured.",
			typeof(TRequest).Name,
			HttpStatusCode.BadRequest,
			validationResult.Errors.ToList());
	}
}