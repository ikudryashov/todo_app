using System.Net;
using TodoApp.Domain.Exceptions.Common.Interfaces;

namespace TodoApp.Domain.Exceptions.RefreshToken.Authentication;

public class InvalidTokenException : Exception, IAuthenticationException
{
	public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public string ErrorMessage => "Provided invalid refresh token.";
}