using System.Net;
using TodoApp.Domain.Exceptions.Common.Interfaces;

namespace TodoApp.Domain.Exceptions.User;

public class UserNotFoundException : Exception, IUserException
{
	public HttpStatusCode StatusCode => HttpStatusCode.NotFound;
	public string ErrorMessage => "The user does not exist.";
}