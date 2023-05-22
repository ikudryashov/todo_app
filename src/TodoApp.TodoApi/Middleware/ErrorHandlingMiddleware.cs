using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Common.Exceptions.Authentication;

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
		var (title, status, detail, type) = exception switch
		{
			DuplicateEmailException => (
				"Failed to create account", StatusCodes.Status400BadRequest, exception.Message, "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"),
			InvalidCredentialsException => (
				"Failed to log in", StatusCodes.Status401Unauthorized, exception.Message, "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3"),
			_ => ("Internal Server Error", StatusCodes.Status500InternalServerError, exception.Message, "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1")
		};

		var problemDetails = new ProblemDetails()
		{
			Instance = context.Request?.Path,
			Status = status,
			Detail = detail,
			Title = title,
			Type = type
		};

		context.Response.StatusCode = problemDetails.Status.Value;
		context.Response.ContentType = "application/problem+json";

		var result =  JsonSerializer.Serialize(problemDetails);

		await context.Response.WriteAsync(result);
	}
}