using System.Net;

namespace TodoApp.Domain.Exceptions.User.Authentication;

public class InvalidCredentialsException : Exception, IAuthenticationException
{
	public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
	public string ErrorMessage => "Invalid credentials provided.";
}