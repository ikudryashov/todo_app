namespace TodoApp.Application.Authentication.Common;

public class RefreshToken
{
	public string Token { get; init; } = null!;
	public DateTime ExpiryDate { get; init; }
}