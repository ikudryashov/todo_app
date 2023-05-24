using System.Net;
using TodoApp.Domain.Exceptions.Common.Interfaces;

namespace TodoApp.Domain.Exceptions.User.Authentication;

public class InvalidCredentialsException : Exception, IAuthenticationException
{
	public HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
	public string ErrorMessage => "User does not exist or provided invalid credentials.";
}