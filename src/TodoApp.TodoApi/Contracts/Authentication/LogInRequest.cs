namespace TodoApp.TodoApi.Contracts;

public record LogInRequest(
		string Email,
		string Password
	);