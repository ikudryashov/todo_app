namespace TodoApp.Application.Common.Exceptions.Authentication;

public class DuplicateEmailException : Exception
{
	public DuplicateEmailException(string message) : base(message) { }
}