namespace TodoApp.TodoApi.Contracts.Authentication;

public record AuthResponse(
		Guid Id,
		string FirstName,
		string LastName,
		string Email,
		string AccessToken,
		string RefreshToken
	);