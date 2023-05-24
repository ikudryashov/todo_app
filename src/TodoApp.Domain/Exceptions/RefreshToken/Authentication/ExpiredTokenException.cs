using System.Net;
using TodoApp.Domain.Exceptions.Common.Interfaces;

namespace TodoApp.Domain.Exceptions.RefreshToken.Authentication;

public class ExpiredTokenException : Exception, IAuthenticationException
{
	public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public string ErrorMessage => "Refresh token is expired and no longer valid.";
}