namespace TodoApp.TodoApi.Contracts.Authentication;

public record LogInRequest(
		string Email,
		string Password
	);