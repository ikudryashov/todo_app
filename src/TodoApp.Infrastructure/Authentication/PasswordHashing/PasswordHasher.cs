using System.Security.Cryptography;
using System.Text;
using TodoApp.Application.Authentication.Common;
using TodoApp.Application.Common.Interfaces.Authentication;

namespace TodoApp.Infrastructure.Authentication.PasswordHashing;

public class PasswordHasher : IPasswordHasher
{
	private const int KeySize = 64;
	private const int Iterations = 35000;
	private readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

	public (string hash, string salt) HashPassword(string password)
	{
		byte[] salt = RandomNumberGenerator.GetBytes(KeySize);

		var hash = Rfc2898DeriveBytes.Pbkdf2(
			Encoding.UTF8.GetBytes(password),
			salt,
			Iterations,
			_hashAlgorithm,
			KeySize);

		return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
	}

	public bool VerifyPassword(string password, string hash, string salt)
	{
		var newHash = Rfc2898DeriveBytes.Pbkdf2(
			Encoding.UTF8.GetBytes(password),
			Convert.FromBase64String(salt),
			Iterations,
			_hashAlgorithm,
			KeySize);

		return CryptographicOperations.FixedTimeEquals(newHash, Convert.FromBase64String(hash));
	}
}