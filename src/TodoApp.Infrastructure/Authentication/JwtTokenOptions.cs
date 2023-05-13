namespace TodoApp.Infrastructure.Authentication;

public class JwtTokenOptions
{
	public const string ConfigSectionName = "JwtTokenOptions";

	public string SecretKey { get; init; } = null!;
	public int ExpiryMinutes { get; init; }
	public string Issuer { get; init; } = null!;
	public string Audience { get; init; } = null!;
}