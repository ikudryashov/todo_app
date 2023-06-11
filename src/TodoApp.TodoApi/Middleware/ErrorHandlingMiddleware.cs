using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TodoApp.Application.Common.Exceptions;

namespace TodoApp.TodoApi.Middleware;

public class ErrorHandlingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ErrorHandlingMiddleware> _logger;

	public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, ex);
		}
	}

	private Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		var problemDetails = exception switch
		{
			ApiException apiException => HandleApiException(context, apiException),
			RequestValidationException validationException => HandleValidationException(context, validationException),
			_ => HandleGenericException(context, exception)
		};
		
		context.Response.StatusCode = problemDetails.Status!.Value;
		context.Response.ContentType = "application/problem+json";
		
		var serializerOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		string result = String.Empty;

		if (problemDetails is ValidationProblemDetails details)
		{
			result = JsonSerializer.Serialize(details, serializerOptions);
		}
		else
		{
			result = JsonSerializer.Serialize(problemDetails, serializerOptions);
		}
		
		return context.Response.WriteAsync(result);
	}

	private ProblemDetails HandleGenericException(HttpContext context, Exception exception)
	{
		_logger.LogError("Request failure: {@Error}, {@DateTimeUTC}",
				exception.Message, DateTime.UtcNow);
		
		return new ProblemDetails
		{			
			Title = "Internal Server Error",
			Detail = "An unexpected error occured. Please try again later.",
			Status = (int)HttpStatusCode.InternalServerError,
			Instance = context.Request.Path,
		};
	}

	private ProblemDetails HandleValidationException(HttpContext context, RequestValidationException exception)
	{
		_logger.LogError("Request failure: {@RequestName}, {@Error}, {@DateTimeUTC}",
			exception.Request, exception.Detail, DateTime.UtcNow);
		
		var modelStateDictionary = new ModelStateDictionary();
		
		foreach (var failure in exception.Failures)
		{
			modelStateDictionary.AddModelError(failure.PropertyName, failure.ErrorMessage);
		}

		return new ValidationProblemDetails(modelStateDictionary)
		{
			Title = exception.Title,
			Detail = exception.Detail,
			Status = (int)exception.StatusCode,
			Instance = context.Request.Path,
		};
	}

	private ProblemDetails HandleApiException(HttpContext context, ApiException exception)
	{
		_logger.LogError("Request failure: {@RequestName}, {@Error}, {@DateTimeUTC}",
			exception.Request, exception.Detail, DateTime.UtcNow);
		
		return new ProblemDetails
		{
			Title = exception.Title,
			Detail = exception.Detail,
			Status = (int)exception.StatusCode,
			Instance = context.Request.Path,
		};
	}
}