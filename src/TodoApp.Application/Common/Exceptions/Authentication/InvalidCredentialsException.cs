using System.Net;

namespace TodoApp.Application.Common.Exceptions.Authentication;

public class InvalidCredentialsException : Exception, IServiceException
{
	public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
	public string ErrorMessage => "Invalid credentials provided.";
}