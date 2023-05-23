namespace TodoApp.TodoApi.Contracts;

public record AuthResponse(
		Guid Id,
		string FirstName,
		string LastName,
		string Email,
		string Token,
		string RefreshToken
	);