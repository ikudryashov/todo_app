using System.Net;

namespace TodoApp.Domain.Exceptions.User.Authentication;

public class DuplicateEmailException : Exception, IAuthenticationException
{
	public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public string ErrorMessage => "This email is already registered";
}