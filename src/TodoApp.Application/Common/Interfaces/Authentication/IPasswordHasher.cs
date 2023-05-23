using TodoApp.Application.Authentication.Common;

namespace TodoApp.Application.Common.Interfaces.Authentication;

public interface IPasswordHasher
{
	public (string hash, byte[] salt)  HashPassword(string password);
	public bool VerifyPassword(string password, string hash, byte[] salt);
}