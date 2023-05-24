using TodoApp.Application.Authentication.Common;

namespace TodoApp.Application.Common.Interfaces.Authentication;

public interface ICredentialsHasher
{
	public (string hash, string salt)  Hash(string plaintext);
	public bool Verify(string plaintext, string hash, string salt);
}