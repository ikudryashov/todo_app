namespace TodoApp.Domain.Entities;

public class User
{
	public Guid Id { get; set; }
	public string FirstName { get; set; } = null!;
	public string LastName { get; set; } = null!;
	public string Email { get; set; } = null!;
	public string Password { get; set; } = null!;
	public byte[] Salt { get; set; } = null!;
}