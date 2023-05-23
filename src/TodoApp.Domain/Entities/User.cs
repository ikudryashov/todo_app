namespace TodoApp.Domain.Entities;

public class User
{
	public Guid Id { get; set; }
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public string Email { get; set; } = null!;
	public string Password { get; set; } = null!;
	public string Salt { get; set; } = null!;
	public string RefreshToken { get; set; } = null!;
	public DateTime RefreshTokenExpiryDate { get; set; }
}