using System.Net;

namespace TodoApp.Domain.Exceptions.Common.Interfaces;

public interface IUserException
{
	public HttpStatusCode StatusCode { get; }
	public string ErrorMessage { get; }
}