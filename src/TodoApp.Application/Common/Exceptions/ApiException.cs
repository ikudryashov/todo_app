using System.Net;
using MediatR;

namespace TodoApp.Application.Common.Exceptions;

public class ApiException : Exception
{
	public string Title { get; }
	public string Detail { get; }
	public string Request { get; }
	public HttpStatusCode StatusCode { get; }

	public ApiException(string title, string detail, string request, HttpStatusCode statusCode)
	{
		Title = title;
		Detail = detail;
		Request = request;
		StatusCode = statusCode;
	}

}