namespace TodoApp.TodoApi.Common.Contracts.Authentication;

public record LogInRequest(
		string Email,
		string Password
	);