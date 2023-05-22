using System.Net;

namespace TodoApp.Application.Common.Exceptions;

public interface IServiceException
{
	public HttpStatusCode StatusCode { get; }
	public string ErrorMessage { get; }
}