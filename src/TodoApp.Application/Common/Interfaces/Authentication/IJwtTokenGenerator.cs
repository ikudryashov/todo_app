namespace TodoApp.Application.Common.Interfaces.Authentication;

public interface IJwtTokenGenerator
{
	public string GenerateToken(Guid id, string firstName, string lastName);
}