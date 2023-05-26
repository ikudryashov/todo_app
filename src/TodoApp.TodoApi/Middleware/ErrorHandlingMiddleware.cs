using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TodoApp.Application.Common.Exceptions;

namespace TodoApp.TodoApi.Middleware;

public class ErrorHandlingMiddleware
{
	private readonly RequestDelegate _next;

	public ErrorHandlingMiddleware(RequestDelegate next)
	{
		_next = next;
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

	private static Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		var problemDetails = exception switch
		{
			ApiException apiException => HandleApiException(context, apiException),
			RequestValidationException validationException => HandleValidationException(context, validationException),
			_ => HandleGenericException(context)
		};
		
		context.Response.StatusCode = problemDetails.Status!.Value;
		context.Response.ContentType = "application/problem+json";
		
		var serializerOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		string result = String.Empty;

		if (problemDetails is ValidationProblemDetails)
		{
			result = JsonSerializer.Serialize(problemDetails as ValidationProblemDetails, serializerOptions);
		}
		else
		{
			result = JsonSerializer.Serialize(problemDetails, serializerOptions);
		}
		
		return context.Response.WriteAsync(result);
	}

	private static ProblemDetails HandleGenericException(HttpContext context)
	{
		return new ProblemDetails
		{			
			Title = "Internal Server Error",
			Detail = "An unexpected error occured. Please try again later.",
			Status = (int)HttpStatusCode.InternalServerError,
			Instance = context.Request.Path,
		};
	}

	private static ProblemDetails HandleValidationException(HttpContext context, RequestValidationException exception)
	{
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

	private static ProblemDetails HandleApiException(HttpContext context, ApiException exception)
	{
		return new ProblemDetails
		{
			Title = exception.Title,
			Detail = exception.Detail,
			Status = (int)exception.StatusCode,
			Instance = context.Request.Path,
		};
	}
}