using System.Reflection.PortableExecutable;

namespace TodoApp.Domain.Entities;

public class RefreshToken
{
	public Guid UserId { get; set; }
	public string Token { get; set; } = null!;
	public string Salt { get; set; } = null!;
	public DateTime ExpiryDate { get; set; }
	public bool IsValid { get; set; }
}