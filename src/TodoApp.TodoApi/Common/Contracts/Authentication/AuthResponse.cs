namespace TodoApp.TodoApi.Common.Contracts.Authentication;

public record AuthResponse(
		Guid UserId,
		string AccessToken,
		string RefreshToken
	);