using System.Net;
using FluentValidation.Results;

namespace TodoApp.Application.Common.Exceptions;

public class RequestValidationException : Exception
{
	public string Title { get; }
	public string Detail { get; }
	public HttpStatusCode StatusCode { get; }
	public List<ValidationFailure> Failures { get;}

	public RequestValidationException(string title, string detail, HttpStatusCode statusCode, List<ValidationFailure> failures)
	{
		Title = title;
		Detail = detail;
		StatusCode = statusCode;
		Failures = failures;
	}
}