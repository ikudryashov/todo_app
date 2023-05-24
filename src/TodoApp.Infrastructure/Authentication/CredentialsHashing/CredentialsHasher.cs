using System.Security.Cryptography;
using System.Text;
using TodoApp.Application.Common.Interfaces.Authentication;

namespace TodoApp.Infrastructure.Authentication.CredentialsHashing;

public class CredentialsHasher : ICredentialsHasher
{
	private const int KeySize = 64;
	private const int Iterations = 35000;
	private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

	public (string hash, string salt) Hash(string plaintext)
	{
		byte[] salt = RandomNumberGenerator.GetBytes(KeySize);

		var hash = Rfc2898DeriveBytes.Pbkdf2(
			Encoding.UTF8.GetBytes(plaintext),
			salt,
			Iterations,
			_hashAlgorithm,
			KeySize);

		return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
	}

	public bool Verify(string plaintext, string hash, string salt)
	{
		var newHash = Rfc2898DeriveBytes.Pbkdf2(
			Encoding.UTF8.GetBytes(plaintext),
			Convert.FromBase64String(salt),
			Iterations,
			_hashAlgorithm,
			KeySize);

		return CryptographicOperations.FixedTimeEquals(newHash, Convert.FromBase64String(hash));
	}
}