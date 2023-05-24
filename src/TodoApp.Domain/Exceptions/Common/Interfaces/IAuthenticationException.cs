using System.Net;

namespace TodoApp.Domain.Exceptions.Common.Interfaces;

public interface IAuthenticationException
{
	public HttpStatusCode StatusCode { get; }
	public string ErrorMessage { get; }
}