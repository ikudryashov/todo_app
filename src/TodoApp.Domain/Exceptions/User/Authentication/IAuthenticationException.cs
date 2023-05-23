using System.Net;

namespace TodoApp.Domain.Exceptions.User.Authentication;

public interface IAuthenticationException
{
	public HttpStatusCode StatusCode { get; }
	public string ErrorMessage { get; }
}