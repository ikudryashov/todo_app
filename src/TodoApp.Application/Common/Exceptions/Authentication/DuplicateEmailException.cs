using System.Net;

namespace TodoApp.Application.Common.Exceptions.Authentication;

public class DuplicateEmailException : Exception, IServiceException
{
	public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
	public string ErrorMessage => "This email is already registered";
}