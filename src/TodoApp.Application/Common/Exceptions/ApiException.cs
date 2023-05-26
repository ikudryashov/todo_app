using System.Net;

namespace TodoApp.Application.Common.Exceptions;

public class ApiException : Exception
{
	public string Title { get; }
	public string Detail { get; }
	public HttpStatusCode StatusCode { get; }

	public ApiException(string title, string detail, HttpStatusCode statusCode)
	{
		Title = title;
		Detail = detail;
		StatusCode = statusCode;
	}

}