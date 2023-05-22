using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
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

	private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		var (title, status, detail) = exception switch
		{
			IServiceException serviceException => (
				"Authentication failed", (int)serviceException.StatusCode, serviceException.ErrorMessage),
			
			_ => ("Internal Server Error", StatusCodes.Status500InternalServerError, 
				"Unexpected error occured. Please try again later.")
		};

		var problemDetails = new ProblemDetails()
		{
			Instance = context.Request?.Path,
			Status = status,
			Detail = detail,
			Title = title,
		};

		context.Response.StatusCode = problemDetails.Status.Value;
		context.Response.ContentType = "application/problem+json";

		var result =  JsonSerializer.Serialize(problemDetails);

		await context.Response.WriteAsync(result);
	}
}