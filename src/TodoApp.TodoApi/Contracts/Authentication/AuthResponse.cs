namespace TodoApp.TodoApi.Contracts.Authentication;

public record AuthResponse(
		Guid UserId,
		string AccessToken,
		string RefreshToken
	);