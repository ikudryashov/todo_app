namespace TodoApp.Application.Common.Exceptions.Authentication;

public class InvalidCredentialsException : Exception
{
	public InvalidCredentialsException(string message) : base(message)
	{ }
}