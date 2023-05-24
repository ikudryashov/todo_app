using System.Net;
using TodoApp.Domain.Exceptions.Common.Interfaces;

namespace TodoApp.Domain.Exceptions.User.Authentication;

public class DuplicateEmailException : Exception, IAuthenticationException
{
	public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public string ErrorMessage => "User with given email already exists.";
}